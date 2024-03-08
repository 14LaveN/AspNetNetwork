using Microsoft.EntityFrameworkCore.Storage;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Domain.Common.Core.Primitives;

namespace AspNetNetwork.Database.Common;

/// <summary>
/// Represents the generic unit of work.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public sealed class UnitOfWork<TEntity>
    : IUnitOfWork<TEntity>
    where TEntity : Entity
{
    private readonly BaseDbContext _baseDbContext;

    /// <summary>
    /// Initialize generic db context.
    /// </summary>
    /// <param name="baseDbContext">The base generic db context.</param>
    public UnitOfWork(BaseDbContext baseDbContext) =>
        _baseDbContext = baseDbContext;

    /// <summary>
    /// Save changes async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The integer result of saving changes.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _baseDbContext.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Begin transaction async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The db context transaction result of begin transaction.</returns>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await _baseDbContext.Database.BeginTransactionAsync(cancellationToken);
}