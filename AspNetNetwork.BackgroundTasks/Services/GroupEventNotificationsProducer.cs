using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Database.Attendee.Data.Interfaces;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Database.Notification.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;

namespace AspNetNetwork.BackgroundTasks.Services;

/// <summary>
/// Represents the group event notifications producer.
/// </summary>
internal sealed class GroupEventNotificationsProducer : IGroupEventNotificationsProducer
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTime _dateTime;
    private readonly IUnitOfWork<Notification> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventNotificationsProducer"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="notificationRepository">The notification repository.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public GroupEventNotificationsProducer(
        IGroupEventRepository groupEventRepository,
        IAttendeeRepository attendeeRepository,
        INotificationRepository notificationRepository,
        IDateTime dateTime,
        IUnitOfWork<Notification> unitOfWork)
    {
        _groupEventRepository = groupEventRepository;
        _attendeeRepository = attendeeRepository;
        _notificationRepository = notificationRepository;
        _dateTime = dateTime;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task ProduceAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        var unprocessedAttendees = await _attendeeRepository.GetUnprocessedAsync(batchSize);

        if (!unprocessedAttendees.Any())
        {
            return;
        }

        var groupEvents = await _groupEventRepository.GetForAttendeesAsync(unprocessedAttendees);

        if (!groupEvents.Any())
        {
            return;
        }

        Dictionary<Guid, GroupEvent> groupEventsDictionary = groupEvents.ToDictionary(g => g.Id, g => g);

        var notifications = new List<Notification>();

        foreach (Attendee attendee in unprocessedAttendees)
        {
            Result result = attendee.MarkAsProcessed();

            if (result.IsFailure)
            {
                continue;
            }

            GroupEvent groupEvent = groupEventsDictionary[attendee.EventId];

            List<Notification> notificationsForAttendee = NotificationType
                .List
                .Select(notificationType => notificationType.TryCreateNotification(groupEvent, attendee.UserId, _dateTime.UtcNow))
                .Where(maybeNotification => maybeNotification.HasValue)
                .Select(maybeNotification => maybeNotification.Value)
                .ToList();
                
            notifications.AddRange(notificationsForAttendee);
        }

        await _notificationRepository.InsertRange(notifications);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}