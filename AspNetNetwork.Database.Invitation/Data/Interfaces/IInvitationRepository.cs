using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.Invitation.Data.Interfaces;

/// <summary>
/// Represents the invitation repository interface.
/// </summary>
public interface IInvitationRepository
{
    /// <summary>
    /// Gets the invitation with the specified identifier.
    /// </summary>
    /// <param name="invitationId">The invitation identifier.</param>
    /// <returns>The maybe instance that may contain the invitation with the specified identifier.</returns>
    Task<Maybe<Domain.Identity.Entities.Invitation>> GetByIdAsync(Guid invitationId);

    /// <summary>
    /// Checks if an invitation for the specified event has already been sent.
    /// </summary>
    /// <param name="groupEvent">The event.</param>
    /// <param name="user">The user.</param>
    /// <returns></returns>
    Task<bool> CheckIfInvitationAlreadySentAsync(Domain.Identity.Entities.GroupEvent groupEvent, User user);

    /// <summary>
    /// Inserts the specified invitation to the database.
    /// </summary>
    /// <param name="invitation">The invitation to be inserted to the database.</param>
    Task Insert(Domain.Identity.Entities.Invitation invitation);

    /// <summary>
    /// Removes all of the invitations for the specified group event.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    /// <returns>The completed task.</returns>
    Task RemoveInvitationsForGroupEventAsync(Domain.Identity.Entities.GroupEvent groupEvent, DateTime utcNow);

    /// <summary>
    /// Invites the specified user to the event.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="user">The user to be invited.</param>
    /// <returns>The result that contains an invitation or an error.</returns>
    Task<Result<Domain.Identity.Entities.Invitation>> InviteAsync(
        Domain.Identity.Entities.GroupEvent groupEvent,
        User user);
}