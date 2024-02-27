using Microsoft.EntityFrameworkCore.Storage;
using AspNetNetwork.Database.Identity.Data.Interfaces;

namespace AspNetNetwork.Database.Identity.Data.Repositories;

/// <summary>
/// Represents the user unit of work.
/// </summary>
public class UserUnitOfWork(UserDbContext userDbContext)
    : IUserUnitOfWork
{
    /// <summary>
    /// Save changes async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The integer result of saving changes.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await userDbContext.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Begin transaction async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The db context transaction result of begin transaction.</returns>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await userDbContext.Database.BeginTransactionAsync(cancellationToken);
}