using AspNetNetwork.Email.Contracts.Emails;

namespace AspNetNetwork.Application.Core.Abstractions.Notifications;

/// <summary>
/// Represents the emailAddress notification service interface.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>
    /// Sends the welcome emailAddress notification based on the specified request.
    /// </summary>
    /// <param name="welcomeEmail">The welcome emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendWelcomeEmail(WelcomeEmail welcomeEmail);
    
    /// <summary>
    /// Sends the done task emailAddress notification based on the specified request.
    /// </summary>
    /// <param name="doneTaskEmail">The done task emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendDoneTaskEmail(DoneTaskEmail doneTaskEmail);

    /// <summary>
    /// Sends the group event cancelled emailAddress.
    /// </summary>
    /// <param name="groupEventCancelledEmail">The group event cancelled emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventCancelledEmail(GroupEventCancelledEmail groupEventCancelledEmail);

    /// <summary>
    /// Sends the group event name changed emailAddress.
    /// </summary>
    /// <param name="groupEventNameChangedEmail">The group event name changed emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventNameChangedEmail(GroupEventNameChangedEmail groupEventNameChangedEmail);

    /// <summary>
    /// Sends the group event date and time changed emailAddress.
    /// </summary>
    /// <param name="groupEventDateAndTimeChangedEmail">The group event date and time changed emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventDateAndTimeChangedEmail(GroupEventDateAndTimeChangedEmail groupEventDateAndTimeChangedEmail);

    /// <summary>
    /// Sends the password changed emailAddress.
    /// </summary>
    /// <param name="passwordChangedEmail">The password changed emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendPasswordChangedEmail(PasswordChangedEmail passwordChangedEmail);

    /// <summary>
    /// Sends the notification emailAddress.
    /// </summary>
    /// <param name="notificationEmail">The notification emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendNotificationEmail(NotificationEmail notificationEmail);
}