using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;


namespace AspNetNetwork.Events.Invitation.Commands.RejectInvitation;

/// <summary>
/// Represents the reject invitation command.
/// </summary>
public sealed class RejectInvitationCommand : ICommand<Result>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RejectInvitationCommand"/> class.
    /// </summary>
    /// <param name="invitationId">The invitation identifier.</param>
    /// <param name="userId">The user id.</param>
    public RejectInvitationCommand(
        Guid invitationId,
        Guid userId)
    {
        InvitationId = invitationId;
        UserId = userId;
    }

    /// <summary>
    /// Gets the invitation identifier.
    /// </summary>
    public Guid InvitationId { get; }
    
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}