using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.User;

/// <summary>
/// Represents the event that is raised when a users password is changed.
/// </summary>
public sealed class UserPasswordChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordChangedDomainEvent"/> class. 
    /// </summary>
    /// <param name="user">The user.</param>
    internal UserPasswordChangedDomainEvent(Entities.User user) => User = user;

    /// <summary>
    /// Gets the user.
    /// </summary>
    public Entities.User User { get; }
}