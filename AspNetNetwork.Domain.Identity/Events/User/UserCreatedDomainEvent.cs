using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.User;

/// <summary>
/// Represents the event that is raised when a user is created.
/// </summary>
public sealed class UserCreatedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="user">The user.</param>
    internal UserCreatedDomainEvent(Entities.User user) => User = user;

    /// <summary>
    /// Gets the user.
    /// </summary>
    public Entities.User User { get; }
}