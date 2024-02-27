using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Events.Invitation.Contracts;

namespace AspNetNetwork.Events.Invitation.Queries.GetSentInvitations;

/// <summary>
/// Represents the query for getting the sent invitations for the user identifier.
/// </summary>
public sealed class GetSentInvitationsQuery : IQuery<Maybe<SentInvitationsListResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSentInvitationsQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier provider.</param>
    public GetSentInvitationsQuery(Guid userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}