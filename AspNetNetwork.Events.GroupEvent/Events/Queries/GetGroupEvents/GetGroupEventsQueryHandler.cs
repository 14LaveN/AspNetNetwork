using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Contracts.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;
using AspNetNetwork.Events.GroupEvent.Contracts.GroupEvents;

namespace AspNetNetwork.Events.GroupEvent.Events.Queries.GetGroupEvents;

/// <summary>
/// Represents the <see cref="GetGroupEventsQuery"/> handler.
/// </summary>
internal sealed class GetGroupEventsQueryHandler : IQueryHandler<GetGroupEventsQuery, Maybe<PagedList<GroupEventResponse>>>
{
    private readonly IDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupEventsQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GetGroupEventsQueryHandler(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<PagedList<GroupEventResponse>>> Handle(GetGroupEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<PagedList<GroupEventResponse>>.None;
        }

        bool shouldSearchCategory = request.CategoryId != null && Category.ContainsValue(request.CategoryId.Value);

        IQueryable<GroupEventResponse> groupEventResponsesQuery =
            from groupEvent in _dbContext.Set<Domain.Identity.Entities.GroupEvent>().AsNoTracking()
            join user in _dbContext.Set<User>().AsNoTracking()
                on groupEvent.UserId equals user.Id
            where groupEvent.UserId == request.UserId &&
                  !groupEvent.Cancelled &&
                  (!shouldSearchCategory || groupEvent.Category.Value == request.CategoryId) &&
                  (request.Name == null || request.Name == "" || groupEvent.Name.Value.Contains(request.Name)) &&
                  (request.StartDate == null || groupEvent.DateTimeUtc >= request.StartDate) &&
                  (request.EndDate == null || groupEvent.DateTimeUtc <= request.EndDate)
            orderby groupEvent.DateTimeUtc descending
            select new GroupEventResponse
            {
                Id = groupEvent.Id,
                Name = groupEvent.Name.Value,
                CategoryId = groupEvent.Category.Value,
                DateTimeUtc = groupEvent.DateTimeUtc,
                CreatedOnUtc = groupEvent.CreatedOnUtc
            };

        int totalCount = await groupEventResponsesQuery.CountAsync(cancellationToken);

        GroupEventResponse[] groupEventResponsesPage = await groupEventResponsesQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        foreach (GroupEventResponse groupEventResponse in groupEventResponsesPage)
        {
            groupEventResponse.Category = Category.FromValue(groupEventResponse.CategoryId).Value.Name;
        }

        return new PagedList<GroupEventResponse>(groupEventResponsesPage, request.Page, request.PageSize, totalCount);
    }
}