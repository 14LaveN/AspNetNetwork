using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.UpdateMessage;

/// <summary>
/// Represents the update message command record class.
/// </summary>
/// <param name="Description">The description.</param>
public sealed record UpdateMessageCommand(
        string Description,
        Guid MessageId)
    : ICommand<IBaseResponse<Result>>;