using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Helpers.JWT;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Database.Message.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.Enumerations;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.UpdateMessage;

/// <summary>
/// Represents the <see cref="UpdateMessageCommand"/> handler class.
/// </summary>
public sealed class UpdateMessageCommandHandler
    : ICommandHandler<UpdateMessageCommand, IBaseResponse<Result>>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly ILogger<UpdateMessageCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMessageCommandHandler"/> class.
    /// </summary>
    /// <param name="messagesRepository">The messages repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    public UpdateMessageCommandHandler(
        IMessagesRepository messagesRepository,
        ILogger<UpdateMessageCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository)
    {
        _messagesRepository = messagesRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
    }
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        UpdateMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for update the message - {request.MessageId} {DateTime.UtcNow}");

            Maybe<User> user = await _userRepository
                .GetByIdAsync(_userIdentifierProvider.UserId);

            if (user.HasNoValue)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }

            Maybe<Message> maybeMessage = await _messagesRepository
                .GetByIdAsync(request.MessageId);

            if (maybeMessage.HasNoValue)
            {
                _logger.LogWarning($"Message with the same identifier - {request.MessageId} not found");
                throw new NotFoundException(nameof(DomainErrors.Message.NotFound), DomainErrors.Message.NotFound);
            }

            Message message = maybeMessage.Value;
            message.Description = request.Description;
            
            Result result = await _messagesRepository.UpdateMessage(message);
            
            if (result.IsSuccess)
                _logger.LogInformation($"Message updated - {message.CreatedOnUtc} {message.Id}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Message updated"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[UpdateMessageCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}