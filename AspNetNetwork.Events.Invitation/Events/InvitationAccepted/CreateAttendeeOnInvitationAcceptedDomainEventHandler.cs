using MediatR;
using AspNetNetwork.Database.Attendee.Data.Interfaces;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Identity.Events.Invitation;
using AspNetNetwork.Events.Attendee.Events.AttendeeCreated;

namespace AspNetNetwork.Events.Invitation.Events.InvitationAccepted;

/// <summary>
/// Represents the <see cref="InvitationAcceptedDomainEvent"/> handler.
/// </summary>
internal sealed class CreateAttendeeOnInvitationAcceptedDomainEventHandler : IDomainEventHandler<InvitationAcceptedDomainEvent>
{
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork<Domain.Identity.Entities.Attendee> _unitOfWork;
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAttendeeOnInvitationAcceptedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="publisher">The publisher.</param>
    public CreateAttendeeOnInvitationAcceptedDomainEventHandler(
        IAttendeeRepository attendeeRepository,
        IUnitOfWork<Domain.Identity.Entities.Attendee> unitOfWork,
        IPublisher publisher)
    {
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    /// <inheritdoc />
    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var attendee = new Domain.Identity.Entities.Attendee(notification.Invitation);
            
        await _attendeeRepository.Insert(attendee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new AttendeeCreatedEvent(attendee.Id), cancellationToken);
    }
}