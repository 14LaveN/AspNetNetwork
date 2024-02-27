using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Events.Invitation.Contracts;

namespace AspNetNetwork.Events.Invitation.Queries.GetPendingInvitations;

/// <summary>
/// Represents the query for getting the pending invitations for the user identifier.
/// </summary>
public sealed class GetPendingInvitationsQuery : IQuery<Maybe<PendingInvitationsListResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPendingInvitationsQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier provider.</param>
    public GetPendingInvitationsQuery(Guid userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}