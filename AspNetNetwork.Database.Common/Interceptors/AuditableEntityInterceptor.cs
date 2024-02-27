using AspNetNetwork.Domain.Common.Core.Abstractions;
using AspNetNetwork.Domain.Common.Core.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspNetNetwork.Database.Common.Interceptors;

public sealed class AuditableEntityInterceptor<T>(
    TimeProvider dateTime) : SaveChangesInterceptor where T : Entity
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DateTimeOffset utcNow = dateTime.GetUtcNow();

        UpdateAuditableEntities(eventData.Context, utcNow);

        if (eventData.Context is not null) 
            UpdateSoftDeletableEntities(eventData.Context, utcNow);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        DateTimeOffset utcNow = dateTime.GetUtcNow();

        UpdateAuditableEntities(eventData.Context, utcNow);

        if (eventData.Context is not null)
            UpdateSoftDeletableEntities(eventData.Context, utcNow);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the entities implementing <see cref="ISoftDeletableEntity"/> interface.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    private void UpdateSoftDeletableEntities(DbContext dbContext, DateTimeOffset utcNow)
    {
        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in dbContext.ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = utcNow;

            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private void UpdateAuditableEntities(DbContext? context, DateTimeOffset utcNow)
    {
        if (context is null) return;

        foreach (var entityEntry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State is EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State is EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
            }
        }
    }
    /// <summary>
    /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
    /// This method is recursive.
    /// </summary>
    /// <param name="entityEntry">The entity entry.</param>
    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
        {
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }
}

