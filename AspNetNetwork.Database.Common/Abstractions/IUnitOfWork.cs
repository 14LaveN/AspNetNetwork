using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetNetwork.Database.Common.Abstractions;

/// <summary>
/// Represents the unit of work interface.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IUnitOfWork<TEntity>
{
    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a transaction on the current unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The new database context transaction.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}