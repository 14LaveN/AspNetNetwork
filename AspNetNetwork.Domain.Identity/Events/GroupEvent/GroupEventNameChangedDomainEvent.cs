﻿using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Identity.Events.GroupEvent;

/// <summary>
/// Represents the event that is raised when the name of a group event is changed.
/// </summary>
public sealed class GroupEventNameChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventNameChangedDomainEvent"/> class.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="previousName">The previous name.</param>
    internal GroupEventNameChangedDomainEvent(Entities.GroupEvent groupEvent, string previousName)
    {
        GroupEvent = groupEvent;
        PreviousName = previousName;
    }

    /// <summary>
    /// Gets the group event.
    /// </summary>
    public Entities.GroupEvent GroupEvent { get; }

    /// <summary>
    /// Gets the previous name.
    /// </summary>
    public string PreviousName { get; }
}