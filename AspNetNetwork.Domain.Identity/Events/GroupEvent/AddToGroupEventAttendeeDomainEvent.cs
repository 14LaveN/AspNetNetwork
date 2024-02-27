using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Domain.Identity.Events.GroupEvent;

/// <summary>
/// Represents the event that is raised when add to group event attendee.
/// </summary>
public sealed class AddToGroupEventAttendeeDomainEvent
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddToGroupEventAttendeeDomainEvent"/> class.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="attendee">The attendee.</param>
    internal AddToGroupEventAttendeeDomainEvent(Entities.GroupEvent groupEvent, Attendee attendee)
    {
        GroupEvent = groupEvent;
        Attendee = attendee;
    }

    /// <summary>
    /// Gets the group event.
    /// </summary>
    public Entities.GroupEvent GroupEvent { get; }
    
    /// <summary>
    /// Gets the attendee.
    /// </summary>
    public Attendee Attendee { get; }
}