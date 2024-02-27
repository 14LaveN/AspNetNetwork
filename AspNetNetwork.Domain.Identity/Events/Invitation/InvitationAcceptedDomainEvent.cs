using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.Invitation;

/// <summary>
/// Represents the event that is raised when an invitation is accepted.
/// </summary>
public sealed class InvitationAcceptedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationAcceptedDomainEvent"/> class.
    /// </summary>
    /// <param name="invitation">The invitation.</param>
    internal InvitationAcceptedDomainEvent(Entities.Invitation invitation) => Invitation = invitation;

    /// <summary>
    /// Gets the invitation.
    /// </summary>
    public Entities.Invitation Invitation { get; }
}