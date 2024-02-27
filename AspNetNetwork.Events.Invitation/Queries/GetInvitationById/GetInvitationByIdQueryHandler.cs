using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Events.Invitation.Contracts;

namespace AspNetNetwork.Events.Invitation.Queries.GetInvitationById;

/// <summary>
/// Represents the <see cref="GetInvitationByIdQuery"/> handler.
/// </summary>
internal sealed class GetInvitationByIdQueryHandler : IQueryHandler<GetInvitationByIdQuery, Maybe<InvitationResponse>>
{
    private readonly BaseDbContext<Domain.Identity.Entities.Invitation> _invitationDbContext;
    private readonly UserDbContext _userDbContext;
    private readonly BaseDbContext<GroupEvent> _groupEventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInvitationByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="invitationDbContext">The invitations database context.</param>
    /// <param name="userDbContext">The users database context.</param>
    /// <param name="groupEventDbContext">The group events database context.</param>
    public GetInvitationByIdQueryHandler(
        BaseDbContext<Domain.Identity.Entities.Invitation> invitationDbContext,
        UserDbContext userDbContext,
        BaseDbContext<GroupEvent> groupEventDbContext)
    {
        _invitationDbContext = invitationDbContext;
        _userDbContext = userDbContext;
        _groupEventDbContext = groupEventDbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<InvitationResponse>> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.InvitationId == Guid.Empty)
        {
            return Maybe<InvitationResponse>.None;
        }

        InvitationResponse? response = await (
            from invitation in _invitationDbContext.Set<Domain.Identity.Entities.Invitation>().AsNoTracking()
            join user in _userDbContext.Set<User>().AsNoTracking()
                on invitation.UserId equals user.Id
            join groupEvent in _groupEventDbContext.Set<GroupEvent>().AsNoTracking()
                on invitation.EventId equals groupEvent.Id
            join friend in _userDbContext.Set<User>().AsNoTracking()
                on groupEvent.UserId equals friend.Id
            where invitation.Id == request.InvitationId &&
                  invitation.UserId == request.UserId &&
                  invitation.CompletedOnUtc == null
            select new InvitationResponse
            {
                Id = invitation.Id,
                EventName = groupEvent.Name.Value,
                EventDateTimeUtc = groupEvent.DateTimeUtc,
                FriendName = friend.FirstName.Value + " " + friend.LastName.Value,
                CreatedOnUtc = invitation.CreatedOnUtc
            }).FirstOrDefaultAsync(cancellationToken);
            
        return response!;
    }
}