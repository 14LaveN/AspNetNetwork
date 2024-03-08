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
    /// Sends the group event cancelled emailAddress.
    /// </summary>
    /// <param name="groupEventCancelledEmail">The group event cancelled emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendGroupEventCancelledEmail(GroupEventCancelledEmail groupEventCancelledEmail);
    
    /// <summary>
    /// Sends the attendee created email.
    /// </summary>
    /// <param name="attendeeCreatedEmail">The attendee created email.</param>
    /// <returns>The completed task.</returns>
    Task SendAttendeeCreatedEmail(AttendeeCreatedEmail attendeeCreatedEmail);

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
    /// Sends the invitation sent email.
    /// </summary>
    /// <param name="invitationSentEmail">The invitation sent email.</param>
    /// <returns>The completed task.</returns>
    Task SendInvitationSentEmail(InvitationSentEmail invitationSentEmail);

    /// <summary>
    /// Sends the invitation accepted email.
    /// </summary>
    /// <param name="invitationAcceptedEmail">The invitation accepted email.</param>
    /// <returns>The completed task.</returns>
    Task SendInvitationAcceptedEmail(InvitationAcceptedEmail invitationAcceptedEmail);

    /// <summary>
    /// Sends the invitation rejected email.
    /// </summary>
    /// <param name="invitationRejectedEmail">The invitation rejected email.</param>
    /// <returns>The completed task.</returns>
    Task SendInvitationRejectedEmail(InvitationRejectedEmail invitationRejectedEmail);

    /// <summary>
    /// Sends the notification emailAddress.
    /// </summary>
    /// <param name="notificationEmail">The notification emailAddress.</param>
    /// <returns>The completed task.</returns>
    Task SendNotificationEmail(NotificationEmail notificationEmail);
}