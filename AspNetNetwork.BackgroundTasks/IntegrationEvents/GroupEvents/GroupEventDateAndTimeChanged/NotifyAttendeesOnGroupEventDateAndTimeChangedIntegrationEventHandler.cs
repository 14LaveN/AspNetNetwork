using System.Globalization;
using AspNetNetwork.Application.Core.Abstractions.Notifications;
using AspNetNetwork.BackgroundTasks.Abstractions.Messaging;
using AspNetNetwork.Database.Attendee.Data.Interfaces;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Email.Contracts.Emails;
using AspNetNetwork.Events.GroupEvent.Events.Events.GroupEventDateAndTimeChanged;

namespace AspNetNetwork.BackgroundTasks.IntegrationEvents.GroupEvents.GroupEventDateAndTimeChanged
{
    /// <summary>
    /// Represents the <see cref="GroupEventDateAndTimeChangedIntegrationEvent"/> class.
    /// </summary>
    internal sealed class NotifyAttendeesOnGroupEventDateAndTimeChangedIntegrationEventHandler
        : IIntegrationEventHandler<GroupEventDateAndTimeChangedIntegrationEvent>
    {
        private readonly IGroupEventRepository _groupEventRepository;
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IEmailNotificationService _emailNotificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyAttendeesOnGroupEventDateAndTimeChangedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="groupEventRepository">The group event repository.</param>
        /// <param name="attendeeRepository">The attendee repository.</param>
        /// <param name="emailNotificationService">The email notification service.</param>
        public NotifyAttendeesOnGroupEventDateAndTimeChangedIntegrationEventHandler(
            IGroupEventRepository groupEventRepository,
            IAttendeeRepository attendeeRepository,
            IEmailNotificationService emailNotificationService)
        {
            _groupEventRepository = groupEventRepository;
            _attendeeRepository = attendeeRepository;
            _emailNotificationService = emailNotificationService;
        }

        /// <inheritdoc />
        public async Task Handle(GroupEventDateAndTimeChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            Maybe<GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(notification.GroupEventId);

            if (maybeGroupEvent.HasNoValue)
            {
                throw new DomainException(DomainErrors.GroupEvent.NotFound);
            }

            GroupEvent groupEvent = maybeGroupEvent.Value;

            (string Email, string Name)[] attendeeEmailsAndNames = await _attendeeRepository.GetEmailsAndNamesForGroupEvent(groupEvent);

            if (attendeeEmailsAndNames.Length == 0)
            {
                return;
            }

            IEnumerable<Task> sendGroupEventCancelledEmailTasks = attendeeEmailsAndNames
                .Select(emailAndName =>
                    new GroupEventDateAndTimeChangedEmail(
                        emailAndName.Email,
                        emailAndName.Name,
                        groupEvent.Name,
                        notification.PreviousDateAndTimeUtc.ToString(CultureInfo.InvariantCulture),
                        groupEvent.DateTimeUtc.ToString(CultureInfo.InvariantCulture)))
                .Select(groupEventDateAndTimeChangedEmail =>
                    _emailNotificationService.SendGroupEventDateAndTimeChangedEmail(groupEventDateAndTimeChangedEmail));

            await Task.WhenAll(sendGroupEventCancelledEmailTasks);
        }
    }
}
