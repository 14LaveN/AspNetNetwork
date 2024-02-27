using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.PersonalEvent;

/// <summary>
/// Represents the event that is a raised when a new personal event is created.
/// </summary>
public sealed class PersonalEventCreatedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="personalEvent">The personal event.</param>
    internal PersonalEventCreatedDomainEvent(Entities.PersonalEvent personalEvent) => PersonalEvent = personalEvent;

    /// <summary>
    /// Gets the personal event.
    /// </summary>
    public Entities.PersonalEvent PersonalEvent { get; }

}