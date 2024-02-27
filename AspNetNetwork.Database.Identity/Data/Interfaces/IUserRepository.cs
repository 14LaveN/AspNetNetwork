using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.Identity.Data.Interfaces;

/// <summary>
/// Represents the user repository interface.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets the user with the specified identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The maybe instance that may contain the user with the specified identifier.</returns>
    Task<Maybe<User>> GetByIdAsync(Guid userId);
    
    /// <summary>
    /// Gets the user with the user name.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <returns>The maybe instance that may contain the user with the specified identifier.</returns>
    Task<Maybe<User>> GetByNameAsync(string name);

    /// <summary>
    /// Gets the user with the specified emailAddress.
    /// </summary>
    /// <param name="emailAddress">The user emailAddress.</param>
    /// <returns>The maybe instance that may contain the user with the specified emailAddress.</returns>
    Task<Maybe<User>> GetByEmailAsync(EmailAddress emailAddress);
}