using System.Text.Json.Serialization;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Identity.Events.Invitation;

namespace AspNetNetwork.Events.Invitation.Events.InvitationSent;

/// <summary>
/// Represents the integration event that is raised when an invitation is sent.
/// </summary>
public sealed class InvitationSentIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationSentIntegrationEvent"/> class.
    /// </summary>
    /// <param name="invitationSentDomainEvent">The invitation sent domain event.</param>
    internal InvitationSentIntegrationEvent(InvitationSentDomainEvent invitationSentDomainEvent) =>
        InvitationId = invitationSentDomainEvent.Invitation.Id;

    [JsonConstructor]
    private InvitationSentIntegrationEvent(Guid invitationId) => InvitationId = invitationId;

    /// <summary>
    /// Gets the invitation identifier.
    /// </summary>
    public Guid InvitationId { get; }
}