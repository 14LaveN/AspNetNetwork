using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.User;

/// <summary>
/// Represents the event that is raised when a users first and last name is changed.
/// </summary>
public sealed class UserNameChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNameChangedDomainEvent"/> class. 
    /// </summary>
    /// <param name="user">The user.</param>
    internal UserNameChangedDomainEvent(Entities.User user) => User = user;

    /// <summary>
    /// Gets the user.
    /// </summary>
    public Entities.User User { get; }
}