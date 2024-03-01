using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.CreateMessage;

/// <summary>
/// Represents the Create message command record class.
/// </summary>
/// <param name="Description">The description.</param>
/// <param name="RecipientId">The recipient identifier.</param>
public sealed record CreateMessageCommand(
        string Description,
        Guid RecipientId)
    : ICommand<IBaseResponse<Result>>
{
    /// <summary>
    /// Create the message entity class from <see cref="CreateMessageCommand"/> record.
    /// </summary>
    /// <param name="command">The create message command.</param>
    /// <returns>The new message entity.</returns>
    public static implicit operator Message(CreateMessageCommand command)
    {
        return new Message(
            command.Description,
            command.RecipientId, 
            Guid.Empty)
        {
            CreatedOnUtc = DateTime.UtcNow
        };
    }
}