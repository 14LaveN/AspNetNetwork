using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.Invitation;

/// <summary>
/// Represents the event that is raised when an invitation is sent.
/// </summary>
public sealed class InvitationSentDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationSentDomainEvent"/> class.
    /// </summary>
    /// <param name="invitation">The invitation.</param>
    public InvitationSentDomainEvent(Entities.Invitation invitation) => Invitation = invitation;

    /// <summary>
    /// Gets the invitation.
    /// </summary>
    public Entities.Invitation Invitation { get; }
}