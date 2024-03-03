using System.Linq.Expressions;
using AspNetNetwork.Database.Common.Specifications;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.Invitation.Data;

/// <summary>
/// Represents the specification for determining the pending invitation.
/// </summary>
internal sealed class PendingInvitationSpecification : Specification<Domain.Identity.Entities.Invitation>
{
    private readonly Guid _groupEventId;
    private readonly Guid _userId;

    internal PendingInvitationSpecification(Domain.Identity.Entities.GroupEvent groupEvent, User user)
    {
        _groupEventId = groupEvent.Id;
        _userId = user.Id;
    }

    /// <inheritdoc />
    public override Expression<Func<Domain.Identity.Entities.Invitation, bool>> ToExpression() =>
        invitation => invitation.CompletedOnUtc == null &&
                      invitation.EventId == _groupEventId &&
                      invitation.UserId == _userId;
}