using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Events.Attendee.Contracts;

namespace AspNetNetwork.Events.Attendee.Queries.GetAttendeesForEventId;

/// <summary>
/// Represents the query for getting group event attendees.
/// </summary>
public sealed class GetAttendeesForGroupEventIdQuery : IQuery<Maybe<AttendeeListResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetAttendeesForGroupEventIdQuery"/> class.
    /// </summary>
    /// <param name="groupEventId">The group event identifier.</param>
    /// <param name="userId">The user identifier.</param>
    public GetAttendeesForGroupEventIdQuery(
        Guid groupEventId,
        Guid userId)
    {
        GroupEventId = groupEventId;
        UserId = userId;
    }

    /// <summary>
    /// Gets the group event identifier.
    /// </summary>
    public Guid GroupEventId { get; }
    
    /// <summary>
    /// Gets the group event identifier.
    /// </summary>
    public Guid UserId { get; }
}