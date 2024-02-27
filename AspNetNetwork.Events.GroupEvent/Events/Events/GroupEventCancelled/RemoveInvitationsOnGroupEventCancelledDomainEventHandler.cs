using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Database.Invitation.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Identity.Events.GroupEvent;

namespace AspNetNetwork.Events.GroupEvent.Events.Events.GroupEventCancelled;

/// <summary>
/// Represents the <see cref="GroupEventCancelledDomainEvent"/> class.
/// </summary>
internal sealed class RemoveInvitationsOnGroupEventCancelledDomainEventHandler : IDomainEventHandler<GroupEventCancelledDomainEvent>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveInvitationsOnGroupEventCancelledDomainEventHandler"/> class.
    /// </summary>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="dateTime">The date and time.</param>
    public RemoveInvitationsOnGroupEventCancelledDomainEventHandler(IInvitationRepository invitationRepository, IDateTime dateTime)
    {
        _invitationRepository = invitationRepository;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task Handle(GroupEventCancelledDomainEvent notification, CancellationToken cancellationToken) =>
        await _invitationRepository.RemoveInvitationsForGroupEventAsync(notification.GroupEvent, _dateTime.UtcNow);
}