using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;
using AspNetNetwork.Events.PersonalEvent.Contracts.PersonalEvents;

namespace AspNetNetwork.Events.PersonalEvent.Events.Queries.GetPersonalEventById;

/// <summary>
/// Represents the <see cref="GetPersonalEventByIdQuery"/> handler.
/// </summary>
internal sealed class GetPersonalEventByIdQueryHandler : IQueryHandler<GetPersonalEventByIdQuery, Maybe<DetailedPersonalEventResponse>>
{
    private readonly IDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonalEventByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GetPersonalEventByIdQueryHandler(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<DetailedPersonalEventResponse>> Handle(
        GetPersonalEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PersonalEventId == Guid.Empty)
        {
            return Maybe<DetailedPersonalEventResponse>.None;
        }

        DetailedPersonalEventResponse? response = await (
            from personalEvent in _dbContext.Set<Domain.Identity.Entities.PersonalEvent>().AsNoTracking()
            join user in _dbContext.Set<User>().AsNoTracking()
                on personalEvent.UserId equals user.Id
            where user.Id == request.UserId &&
                  personalEvent.Id == request.PersonalEventId &&
                  !personalEvent.Cancelled
            select new DetailedPersonalEventResponse
            {
                Id = personalEvent.Id,
                Name = personalEvent.Name.Value,
                CategoryId = personalEvent.Category.Value,
                CreatedBy = user.FirstName.Value + " " + user.LastName.Value,
                DateTimeUtc = personalEvent.DateTimeUtc,
                CreatedOnUtc = personalEvent.CreatedOnUtc
            }).FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Maybe<DetailedPersonalEventResponse>.None;
        }

        response.Category = Category.FromValue(response.CategoryId).Value.Name;
            
        return response;
    }
}