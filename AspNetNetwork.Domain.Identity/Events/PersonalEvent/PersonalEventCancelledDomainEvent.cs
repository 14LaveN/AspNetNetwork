using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.PersonalEvent;

/// <summary>
/// Represents the event that is raised when a personal event is cancelled.
/// </summary>
public sealed class PersonalEventCancelledDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventCancelledDomainEvent"/> class.
    /// </summary>
    /// <param name="personalEvent">The personal event.</param>
    internal PersonalEventCancelledDomainEvent(Entities.PersonalEvent personalEvent) => PersonalEvent = personalEvent;

    /// <summary>
    /// Gets the personal event.
    /// </summary>
    public Entities.PersonalEvent PersonalEvent { get; }
}