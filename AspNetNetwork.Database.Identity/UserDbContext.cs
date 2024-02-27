using System.Reflection;
using AspNetNetwork.Domain.Common.Core.Abstractions;
using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Common.Core.Primitives;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.Identity;

/// <summary>
/// Represents the application database context identity class.
/// </summary>
public class UserDbContext
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityDbContext"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR.</param>
    /// <param name="dbContextOptions">The database context options.</param>
    public UserDbContext(IMediator mediator,
        DbContextOptions<UserDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public UserDbContext()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=TTGenericDb;User Id=postgres;Password=1111;");
    }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.HasDefaultSchema("dbo");
        
        
        modelBuilder.Entity<IdentityUserLogin<long>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });
        
        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc cref="int"/>
    public async Task<int> ExecuteSqlAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default)
        => await Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    /// <exception cref="ArgumentNullException"></exception>
    /// <inheritdoc cref="User" />
    public async Task<Maybe<User>> GetBydIdAsync(Guid id)
        => id == Guid.Empty ?
            Maybe<User>.None :
            Maybe<User>.From(await Set<User>().FirstOrDefaultAsync(e => e.Id == id) 
                             ?? throw new ArgumentNullException());

    /// <inheritdoc cref="Task" />
    public async Task Insert<TEntity>(TEntity entity)
        where TEntity : Entity
        => await Set<TEntity>().AddAsync(entity);

    /// <inheritdoc cref="Task"/>
    public async Task InsertRange(IReadOnlyCollection<User> entities)
        => await base.Set<User>().AddRangeAsync(entities);

    /// <inheritdoc cref="Void" />
    public void Remove(User entity)
        => Set<User>().Remove(entity);
    
   /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime utcNow = DateTime.UtcNow;

            UpdateAuditableEntities(utcNow);

            UpdateSoftDeletableEntities(utcNow);

            await PublishDomainEvents(cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the entities implementing <see cref="IAuditableEntity"/> interface.
        /// </summary>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        private void UpdateAuditableEntities(DateTime utcNow)
        {
            foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
                }
            }
        }

        /// <summary>
        /// Updates the entities implementing <see cref="ISoftDeletableEntity"/> interface.
        /// </summary>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        private void UpdateSoftDeletableEntities(DateTime utcNow)
        {
            foreach (EntityEntry<ISoftDeletableEntity> entityEntry in ChangeTracker.Entries<ISoftDeletableEntity>())
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

            foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry is not null && r.TargetEntry.State == EntityState.Deleted))
            {
                if (referenceEntry.TargetEntry != null)
                {
                    referenceEntry.TargetEntry.State = EntityState.Unchanged;

                    UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
                }
            }
        }

        /// <summary>
        /// Publishes and then clears all the domain events that exist within the current transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            List<EntityEntry<User>> aggregateRoots = ChangeTracker
                .Entries<User>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents.Count is not 0)
                .ToList();

            List<IDomainEvent> domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            IEnumerable<Task> tasks = domainEvents.Select(async domainEvent => 
                await _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(tasks);
        }
}