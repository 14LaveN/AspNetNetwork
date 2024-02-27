using AspNetNetwork.Application.Core.Abstractions.Notifications;
using AspNetNetwork.BackgroundTasks.Abstractions.Messaging;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Email.Contracts.Emails;
using AspNetNetwork.RabbitMq.Messaging.User.Events.PasswordChanged;

namespace AspNetNetwork.BackgroundTasks.IntegrationEvents.Users.UserPasswordChanged;

/// <summary>
/// Represents the <see cref="UserPasswordChangedIntegrationEvent"/> handler.
/// </summary>
internal sealed class NotifyUserOnPasswordChangedIntegrationEventHandler
    : IIntegrationEventHandler<UserPasswordChangedIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotifyUserOnPasswordChangedIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public NotifyUserOnPasswordChangedIntegrationEventHandler(
        IUserRepository userRepository,
        IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task Handle(UserPasswordChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(notification.UserId);

        if (maybeUser.HasNoValue)
        {
            throw new DomainException(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var passwordChangedEmail = new PasswordChangedEmail(user.EmailAddress, user.FullName);

        await _emailNotificationService.SendPasswordChangedEmail(passwordChangedEmail);
    }
}