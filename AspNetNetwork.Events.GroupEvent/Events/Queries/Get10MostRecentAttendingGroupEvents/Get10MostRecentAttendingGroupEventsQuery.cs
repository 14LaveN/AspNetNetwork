﻿using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Events.GroupEvent.Contracts.GroupEvents;

namespace AspNetNetwork.Events.GroupEvent.Events.Queries.Get10MostRecentAttendingGroupEvents;

/// <summary>
/// Represents the query for getting the 10 most recent group event the user is attending.
/// </summary>
public sealed class Get10MostRecentAttendingGroupEventsQuery : IQuery<Maybe<IReadOnlyCollection<GroupEventResponse>>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Get10MostRecentAttendingGroupEventsQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    public Get10MostRecentAttendingGroupEventsQuery(Guid userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the number of group events to take.
    /// </summary>
    public int NumberOfGroupEventsToTake => 10;
}