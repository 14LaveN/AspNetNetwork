﻿using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;
using AspNetNetwork.Events.GroupEvent.Contracts.GroupEvents;

namespace AspNetNetwork.Events.GroupEvent.Events.Queries.GetGroupEventById;

/// <summary>
/// Represents the <see cref="GetGroupEventByIdQuery"/> handler.
/// </summary>
internal sealed class GetGroupEventByIdQueryHandler : IQueryHandler<GetGroupEventByIdQuery, Maybe<DetailedGroupEventResponse>>
{
    private readonly IDbContext<Domain.Identity.Entities.GroupEvent> _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupEventByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GetGroupEventByIdQueryHandler(IDbContext<Domain.Identity.Entities.GroupEvent> dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Checks if the current user has permissions to query this group event.
    /// </summary>
    /// <param name="groupEventId">The group event identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>True if the current user is the group event owner or an attendee.</returns>
    private async Task<bool> HasPermissionsToQueryGroupEvent(Guid groupEventId, Guid userId) =>
        await (
            from attendee in _dbContext.Set<Attendee>().AsNoTracking()
            join groupEvent in _dbContext.Set<Domain.Identity.Entities.GroupEvent>().AsNoTracking()
                on attendee.EventId equals groupEvent.Id
            where groupEvent.Id == groupEventId &&
                  !groupEvent.Cancelled &&
                  (groupEvent.UserId == userId ||
                   attendee.UserId == userId)
            select true).AnyAsync();

    /// <inheritdoc />
    public async Task<Maybe<DetailedGroupEventResponse>> Handle(
        GetGroupEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.GroupEventId == Guid.Empty 
            || !await HasPermissionsToQueryGroupEvent(request.GroupEventId, request.UserId))
        {
            return Maybe<DetailedGroupEventResponse>.None;
        }

        DetailedGroupEventResponse? response = await (
            from groupEvent in _dbContext.Set<Domain.Identity.Entities.GroupEvent>().AsNoTracking()
            join user in _dbContext.Set<User>().AsNoTracking()
                on groupEvent.UserId equals user.Id
            where groupEvent.Id == request.GroupEventId && !groupEvent.Cancelled
            select new DetailedGroupEventResponse
            {
                Id = groupEvent.Id,
                Name = groupEvent.Name.Value,
                CategoryId = groupEvent.Category.Value,
                CreatedBy = user.FirstName.Value + " " + user.LastName.Value,
                DateTimeUtc = groupEvent.DateTimeUtc,
                CreatedOnUtc = groupEvent.CreatedOnUtc
            }).FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Maybe<DetailedGroupEventResponse>.None;
        }

        response.Category = Category.FromValue(response.CategoryId).Value.Name;

        response.NumberOfAttendees = await _dbContext.Set<Attendee>()
            .Where(x => x.EventId == response.Id)
            .CountAsync(cancellationToken);

        return response;
    }
}