using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.PersonalEvent;

/// <summary>
/// Represents the event that is raised when the date and time of a personal event is changed.
/// </summary>
public sealed class PersonalEventDateAndTimeChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventDateAndTimeChangedDomainEvent"/> class.
    /// </summary>
    /// <param name="personalEvent">The personal event.</param>
    internal PersonalEventDateAndTimeChangedDomainEvent(Entities.PersonalEvent personalEvent) => PersonalEvent = personalEvent;

    /// <summary>
    /// Gets the personal event.
    /// </summary>
    public Entities.PersonalEvent PersonalEvent { get; }
}