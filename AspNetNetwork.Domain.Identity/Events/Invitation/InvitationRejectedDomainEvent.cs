using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.Invitation;

/// <summary>
/// Represents the event that is raised when an invitation is rejected.
/// </summary>
public sealed class InvitationRejectedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationRejectedDomainEvent"/> class.
    /// </summary>
    /// <param name="invitation">The invitation.</param>
    internal InvitationRejectedDomainEvent(Entities.Invitation invitation) => Invitation = invitation;

    /// <summary>
    /// Gets the invitation.
    /// </summary>
    public Entities.Invitation Invitation { get; }
}