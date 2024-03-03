using AspNetNetwork.Database.Attendee.Data.Interfaces;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Database.Invitation.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Message.Events;
using AspNetNetwork.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;
using AspNetNetwork.Events.GroupEvent.Events.Commands.CreateGroupEvent;
using AspNetNetwork.RabbitMq.Messaging.Messages.Events.Create;
using MediatR;

namespace AspNetNetwork.Micro.MessagingAPI.Events.Create;

/// <summary>
/// Represents the <see cref="MessageCreatedIntegrationEvent"/> handler class.
/// </summary>
internal sealed class AddAttendeeAndInvitationOnMessageCreatedDomainEventHandler
    : IDomainEventHandler<MessageCreatedDomainEvent>
{
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork<GroupEvent> _unitOfWork;
    private readonly ISender _sender;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork<Attendee> _attendeeUnitOfWork;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork<Invitation> _invitationUnitOfWork;
    private readonly ILogger<AddAttendeeAndInvitationOnMessageCreatedDomainEventHandler> _logger;
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAttendeeAndInvitationOnMessageCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="invitationUnitOfWork">The invitation unit of work.</param>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="attendeeUnitOfWork">The attendee unit of work.</param>
    /// <param name="publisher">The publisher.</param>
    public AddAttendeeAndInvitationOnMessageCreatedDomainEventHandler(
        IAttendeeRepository attendeeRepository,
        IUnitOfWork<GroupEvent> unitOfWork,
        IUnitOfWork<Invitation> invitationUnitOfWork,
        IInvitationRepository invitationRepository,
        IGroupEventRepository groupEventRepository,
        ISender sender,
        ILogger<AddAttendeeAndInvitationOnMessageCreatedDomainEventHandler> logger,
        IUnitOfWork<Attendee> attendeeUnitOfWork, 
        IPublisher publisher)
    {
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
        _invitationUnitOfWork = invitationUnitOfWork;
        _invitationRepository = invitationRepository;
        _groupEventRepository = groupEventRepository;
        _sender = sender;
        _logger = logger;
        _attendeeUnitOfWork = attendeeUnitOfWork;
        _publisher = publisher;
    }
    
    /// <inheritdoc />
    public async Task Handle(MessageCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var resultGroupEvent = await _sender.Send(new CreateGroupEventCommand(
            notification.Message.AuthorId,
            "CreateMessage.GroupEvent",
            1,
            DateTime.UtcNow), cancellationToken);

        if (resultGroupEvent.IsFailure)
        {
            _logger.LogWarning(DomainErrors.GroupEvent.NotFound);
            throw new DomainException(DomainErrors.GroupEvent.NotFound);
        }

        Maybe<GroupEvent> groupEvent = await _groupEventRepository
            .GetGroupEventByName("CreateMessage.GroupEvent");
            
        if (groupEvent.HasNoValue)
        {
            _logger.LogWarning($"Your group event not found by name - CreateMessage.GroupEvent");

            throw new NotFoundException(nameof(DomainErrors.GroupEvent.NotFound), DomainErrors.GroupEvent.NotFound);
        }
            
        Result<Invitation> invitationResult = await _invitationRepository.InviteAsync(groupEvent,notification.Message.Recipient!);

        if (invitationResult.IsFailure)
        {
            throw new ArgumentException(invitationResult.Error);
        }

        Invitation invitation = invitationResult.Value;

        var acceptResult = invitation.Accept(DateTime.UtcNow);
        if (acceptResult.IsSuccess)
        {
            await _invitationRepository.Insert(invitation);
            await _invitationUnitOfWork.SaveChangesAsync(cancellationToken);
        }

        Attendee attendee = new Attendee(invitation);

        var addToGroupEventAttendeeResult = await GroupEvent.AddToGroupEventAttendee(
            groupEvent, attendee);

        if (addToGroupEventAttendeeResult.IsFailure)
        {
            _logger.LogWarning(DomainErrors.GroupEvent.NotFound + $"by the id-{groupEvent.Value.Id}");
            throw new Exception(DomainErrors.GroupEvent.NotFound);
        }
            
        groupEvent.Value.Attendees?.Add(attendee);
        
        await _attendeeRepository.Insert(attendee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _attendeeUnitOfWork.SaveChangesAsync(cancellationToken);    
        
        _logger.LogInformation($"Add to group event attendee - {groupEvent.Value.Name} {groupEvent.Value.CreatedOnUtc}");

        await _publisher.Publish(new MessageCreatedDomainEvent(notification.Message), cancellationToken);
    }
}