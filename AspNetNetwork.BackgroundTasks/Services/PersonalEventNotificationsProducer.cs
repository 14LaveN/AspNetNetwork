﻿using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Notification.Data.Interfaces;
using AspNetNetwork.Database.PersonalEvent.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;

namespace AspNetNetwork.BackgroundTasks.Services;

//TODO Create Publishers in Personal and Group events.
//TODO Create publishers where events saving and cancelled.
//TODO Create publishers notification.

/// <summary>
/// Represents the personal event notifications producer.
/// </summary>
internal sealed class PersonalEventNotificationsProducer : IPersonalEventNotificationsProducer
{
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTime _dateTime;
    private readonly IUnitOfWork<Notification> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventNotificationsProducer"/> class.
    /// </summary>
    /// <param name="personalEventRepository">The personal event repository.</param>
    /// <param name="notificationRepository">The notification repository.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PersonalEventNotificationsProducer(
        IPersonalEventRepository personalEventRepository,
        INotificationRepository notificationRepository,
        IDateTime dateTime,
        IUnitOfWork<Notification> unitOfWork)
    {
        _personalEventRepository = personalEventRepository;
        _notificationRepository = notificationRepository;
        _dateTime = dateTime;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task ProduceAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<PersonalEvent> unprocessedPersonalEvents = await _personalEventRepository.GetUnprocessedAsync(batchSize);

        if (!unprocessedPersonalEvents.Any())
        {
            return;
        }

        var notifications = new List<Notification>();

        foreach (var personalEvent in unprocessedPersonalEvents)
        {
            Result result = personalEvent.MarkAsProcessed();

            if (result.IsFailure)
            {
                continue;
            }

            List<Notification> notificationsForPersonalEvent = NotificationType
                .List
                .Select(notificationType => notificationType.TryCreateNotification(personalEvent, personalEvent.UserId, _dateTime.UtcNow))
                .Where(maybeNotification => maybeNotification.HasValue)
                .Select(maybeNotification => maybeNotification.Value)
                .ToList();

            notifications.AddRange(notificationsForPersonalEvent);
        }

        await _notificationRepository.InsertRange(notifications);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}