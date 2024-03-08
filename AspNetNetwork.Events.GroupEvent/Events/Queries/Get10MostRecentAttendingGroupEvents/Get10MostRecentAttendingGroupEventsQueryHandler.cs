using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;
using AspNetNetwork.Events.GroupEvent.Contracts.GroupEvents;

namespace AspNetNetwork.Events.GroupEvent.Events.Queries.Get10MostRecentAttendingGroupEvents;

/// <summary>
/// Represents the <see cref="Get10MostRecentAttendingGroupEventsQuery"/> handler.
/// </summary>
internal sealed class Get10MostRecentAttendingGroupEventsQueryHandler
    : IQueryHandler<Get10MostRecentAttendingGroupEventsQuery, Maybe<IReadOnlyCollection<GroupEventResponse>>>
{
    private readonly IDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="Get10MostRecentAttendingGroupEventsQueryHandler"/> class.
    /// </summary>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="dbContext">The database context.</param>
    public Get10MostRecentAttendingGroupEventsQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<IReadOnlyCollection<GroupEventResponse>>> Handle(
        Get10MostRecentAttendingGroupEventsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<IReadOnlyCollection<GroupEventResponse>>.None;
        }

        GroupEventResponse[] responses = await (
                from attendee in _dbContext.Set<Attendee>().AsNoTracking()
                join groupEvent in _dbContext.Set<Domain.Identity.Entities.GroupEvent>().AsNoTracking()
                    on attendee.EventId equals groupEvent.Id
                where attendee.UserId == request.UserId
                orderby groupEvent.DateTimeUtc
                select new GroupEventResponse
                {
                    Id = groupEvent.Id,
                    Name = groupEvent.Name.Value,
                    CategoryId = groupEvent.Category.Value,
                    DateTimeUtc = groupEvent.DateTimeUtc,
                    CreatedOnUtc = groupEvent.CreatedOnUtc
                })
            .Take(request.NumberOfGroupEventsToTake)
            .ToArrayAsync(cancellationToken);

        foreach (GroupEventResponse groupEventResponse in responses)
        {
            groupEventResponse.Category = Category.FromValue(groupEventResponse.CategoryId).Value.Name;
        }

        return responses;
    }
}