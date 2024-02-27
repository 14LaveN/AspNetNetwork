using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Events.GroupEvent.Contracts.GroupEvents;

namespace AspNetNetwork.Events.GroupEvent.Events.Queries.GetGroupEventById;

/// <summary>
/// Represents the query for getting the group event by identifier.
/// </summary>
public sealed class GetGroupEventByIdQuery : IQuery<Maybe<DetailedGroupEventResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupEventByIdQuery"/> class.
    /// </summary>
    /// <param name="groupEventId">The group event identifier.</param>
    /// <param name="userId">The user id identifier.</param>
    public GetGroupEventByIdQuery(Guid groupEventId,
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
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}