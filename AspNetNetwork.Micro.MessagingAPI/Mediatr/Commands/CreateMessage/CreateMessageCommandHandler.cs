using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Helpers.JWT;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Database.Message.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.Enumerations;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Message.Entities;

namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.CreateMessage;

/// <summary>
/// Represents the create message command handler class.
/// </summary>
public sealed class CreateMessageCommandHandler 
    : ICommandHandler<CreateMessageCommand, IBaseResponse<Result>>
{
    
    private readonly IMessagesRepository _messagesRepository;
    private readonly ILogger<CreateMessageCommandHandler> _logger;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMessageCommandHandler"/> class.
    /// </summary>
    /// <param name="messagesRepository">The messages repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    public CreateMessageCommandHandler(
        IMessagesRepository messagesRepository,
        ILogger<CreateMessageCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository)
    {
        _messagesRepository = messagesRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
    }
    
    //TODO Create group event which create relationships with Author and Recipient.
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for create the message - {request.Description} {DateTime.UtcNow}");

            Maybe<User> user = await _userRepository
                .GetByIdAsync(_userIdentifierProvider.UserId);
            
            if (user.HasNoValue)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }
            
            Message message = request;
            
            message = message.Create(
                request.Description,
                _userIdentifierProvider.UserId,
                request.RecipientId);

            message.CreatedOnUtc = DateTime.UtcNow;
            
            await _messagesRepository.Insert(message);
            
            _logger.LogInformation($"Message created - {message.CreatedOnUtc} {message.Id}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Message created"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[CreateMessageCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}