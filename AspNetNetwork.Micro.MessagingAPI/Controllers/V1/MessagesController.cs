using AspNetNetwork.Application.ApiHelpers.Contracts;
using AspNetNetwork.Application.ApiHelpers.Infrastructure;
using AspNetNetwork.Application.ApiHelpers.Policy;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Micro.MessagingAPI.Contracts.Message.Create;
using AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.CreateMessage;
using AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.UpdateMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetNetwork.Micro.MessagingAPI.Controllers.V1;

/// <summary>
/// Represents the messages controller class.
/// </summary>
[Route("api/v1/messages")]
public sealed class MessagesController(
        ISender sender,
        IUserRepository userRepository)
    : ApiController(sender, userRepository)
{
    /// <summary>
    /// Create message.
    /// </summary>
    /// <param name="request">The <see cref="CreateMessageRequest"/> class.</param>
    /// <returns>Base information about create message method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Message.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(createMessageRequest => new CreateMessageCommand(createMessageRequest.Description, createMessageRequest.RecipientId))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);
    
    /// <summary>
    /// Update message.
    /// </summary>
    /// <param name="request">The description.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <returns>Base information about update message method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">BadRequest.</response>
    /// <response code="500">Internal server error</response>
    [HttpPatch(ApiRoutes.Task.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTask([FromBody] string request,
        [FromRoute] Guid messageId) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(updateMessageRequest => new UpdateMessageCommand(
                    updateMessageRequest,
                    messageId))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);
}