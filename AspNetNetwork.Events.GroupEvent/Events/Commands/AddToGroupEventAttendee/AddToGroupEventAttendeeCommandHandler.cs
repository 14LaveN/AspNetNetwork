using Microsoft.Extensions.Logging;
using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Helpers.JWT;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.Enumerations;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;

/// <summary>
/// Represents the <see cref="AddToGroupEventAttendeeCommandHandler"/> class.
/// </summary>
internal sealed class AddToGroupEventAttendeeCommandHandler
    : ICommandHandler<AddToGroupEventAttendeeCommand, IBaseResponse<Result>>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly ILogger<AddToGroupEventAttendeeCommandHandler> _logger;
    private readonly IUnitOfWork<Domain.Identity.Entities.GroupEvent> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddToGroupEventAttendeeCommandHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="logger">The logger.</param>
    public AddToGroupEventAttendeeCommandHandler(
        IGroupEventRepository groupEventRepository,
        IUnitOfWork<Domain.Identity.Entities.GroupEvent> unitOfWork,
        IUserIdentifierProvider userIdentifierProvider,
        ILogger<AddToGroupEventAttendeeCommandHandler> logger)
    {
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        AddToGroupEventAttendeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for add to group event attendee - {request.Attendee} {DateTime.UtcNow}");
            
            if (_userIdentifierProvider.UserId == Guid.Empty)
            {
                _logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }
            
            Maybe<Domain.Identity.Entities.GroupEvent> maybeGroupEvent = await _groupEventRepository
                .GetByIdAsync(request.GroupEventId);

            if (maybeGroupEvent.HasNoValue)
            {
                _logger.LogWarning($"Group event with same identifier - {request.GroupEventId} not found.");
                throw new NotFoundException(nameof(DomainErrors.GroupEvent.NotFound), DomainErrors.GroupEvent.NotFound);
            }

            Domain.Identity.Entities.GroupEvent groupEvent = maybeGroupEvent.Value;
            
            var addToGroupEventAttendeeResult = await Domain.Identity.Entities.GroupEvent.AddToGroupEventAttendee(groupEvent, request.Attendee);

            if (addToGroupEventAttendeeResult.IsFailure)
            {
                _logger.LogWarning(DomainErrors.Attendee.NotFound + $"by the id-{request.Attendee.Id}");
                throw new Exception(DomainErrors.Attendee.NotFound);
            }
            
            groupEvent.Attendees?.Add(request.Attendee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation($"Add to group event attendee - {groupEvent.Name} {groupEvent.CreatedOnUtc}");

            return new BaseResponse<Result>
            {
                Data = Result.Success(),
                StatusCode = StatusCode.Ok,
                Description = "Add to group event attendee"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[AddToGroupEventAttendeeCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}