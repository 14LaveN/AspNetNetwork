using System.Text.Json.Serialization;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Identity.Events.GroupEvent;

namespace AspNetNetwork.Events.GroupEvent.Events.Events.AddToGroupEventAttendee;

/// <summary>
/// Represents the event that is raised when add attendee to group event.
/// </summary>
public sealed class AddToGroupEventAttendeeIntegrationEvent
    : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddToGroupEventAttendeeIntegrationEvent"/> class.
    /// </summary>
    /// <param name="groupEventAttendeeDomain">The add to group event attendee domain event.</param>
    internal AddToGroupEventAttendeeIntegrationEvent(AddToGroupEventAttendeeDomainEvent groupEventAttendeeDomain) =>
        GroupEventId = groupEventAttendeeDomain.GroupEvent.Id;

    [JsonConstructor]
    private AddToGroupEventAttendeeIntegrationEvent(Guid groupEventId) => GroupEventId = groupEventId;

    /// <summary>
    /// Gets the group event identifier.
    /// </summary>
    public Guid GroupEventId { get; }
}