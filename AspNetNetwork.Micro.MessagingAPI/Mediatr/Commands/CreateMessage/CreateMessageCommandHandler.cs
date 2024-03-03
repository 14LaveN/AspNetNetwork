using System.Runtime.Intrinsics.Arm;
using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Helpers.JWT;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Database.Invitation.Data.Interfaces;
using AspNetNetwork.Database.Message.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.Enumerations;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Message.Entities;
using AspNetNetwork.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;
using AspNetNetwork.Events.GroupEvent.Events.Commands.CreateGroupEvent;
using MediatR;

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
    private readonly ISender _sender;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork<Invitation> _invitationUnitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMessageCommandHandler"/> class.
    /// </summary>
    /// <param name="messagesRepository">The messages repository.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="invitationUnitOfWork">The invitation unit of work.</param>
    public CreateMessageCommandHandler(
        IMessagesRepository messagesRepository,
        ILogger<CreateMessageCommandHandler> logger,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository, 
        ISender sender,
        IGroupEventRepository groupEventRepository,
        IInvitationRepository invitationRepository,
        IUnitOfWork<Invitation> invitationUnitOfWork)
    {
        _messagesRepository = messagesRepository;
        _logger = logger;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
        _sender = sender;
        _groupEventRepository = groupEventRepository;
        _invitationRepository = invitationRepository;
        _invitationUnitOfWork = invitationUnitOfWork;
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
            
            Maybe<User> recipient = await _userRepository
                .GetByIdAsync(request.RecipientId);
            
            if (recipient.HasNoValue)
            {
                _logger.LogWarning($"Not found the recipient with id - {request.RecipientId}");
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