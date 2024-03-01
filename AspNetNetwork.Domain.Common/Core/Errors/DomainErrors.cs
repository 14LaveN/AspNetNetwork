using AspNetNetwork.Domain.Common.Core.Primitives;

namespace AspNetNetwork.Domain.Common.Core.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
public static class DomainErrors
{
    /// <summary>
    /// Contains the company errors.
    /// </summary>
    public static class Company
    {
        public static Error NotFound =>
            new("Company.NotFound", "The company with the specified identifier was not found.");
        
        public static Error AlreadyDone => new("Task.AlreadyDone", "The task with the specified identifier already done.");
    }
    
    /// <summary>
    /// Contains the message errors.
    /// </summary>
    public static class Message
    {
        public static Error NotFound =>
            new("Message.NotFound", "The message with the specified identifier was not found.");
        
        public static Error AlreadyAnswered => new("Message.AlreadyAnswered", "The message with the specified identifier already answered.");
    }


    /// <summary>
    /// Contains the user errors.
    /// </summary>
    public static class User
    {
        public static Error NotFound => new("User.NotFound", "The user with the specified identifier was not found.");

        public static Error InvalidPermissions => new(
            "User.InvalidPermissions",
            "The current user does not have the permissions to perform that operation.");
            
        public static Error DuplicateEmail => new("User.DuplicateEmail", "The specified emailAddress is already in use.");

        public static Error CannotChangePassword => new(
            "User.CannotChangePassword",
            "The password cannot be changed to the specified password.");
    }

    /// <summary>
    /// Contains the attendee errors.
    /// </summary>
    public static class Attendee
    {
        public static Error NotFound => new("Attendee.NotFound", "The attendee with the specified identifier was not found.");

        public static Error AlreadyProcessed => new("Attendee.AlreadyProcessed", "The attendee has already been processed.");
    }

    /// <summary>
    /// Contains the category errors.
    /// </summary>
    public static class Category
    {
        public static Error NotFound => new("Category.NotFound", "The category with the specified identifier was not found.");
    }

    /// <summary>
    /// Contains the event errors.
    /// </summary>
    public static class Event
    {
        public static Error AlreadyCancelled => new("Event.AlreadyCancelled", "The event has already been cancelled.");

        public static Error EventHasPassed => new(
            "Event.EventHasPassed",
            "The event has already passed and cannot be modified.");
    }

    /// <summary>
    /// Contains the group event errors.
    /// </summary>
    public static class GroupEvent
    {
        public static Error AlreadyProcessed =>
            new("GroupEvent.AlreadyProcessed", "The event has already been processed.");
        
        public static Error NotFound => new(
            "GroupEvent.NotFound",
            "The group event with the specified identifier was not found.");
        
        public static Error IsCancelled => new(
            "GroupEvent.IsCancelled",
            "The group event with the specified identifier was cancelled.");

        public static Error UserNotFound => new(
            "GroupEvent.UserNotFound",
            "The user with the specified identifier was not found.");

        public static Error FriendNotFound => new(
            "GroupEvent.FriendNotFound",
            "The friend with the specified identifier was not found.");

        public static Error InvitationAlreadySent => new(
            "GroupEvent.InvitationAlreadySent",
            "The invitation for this event has already been sent to this user.");

        public static Error NotFriends => new(
            "GroupEvent.NotFriends",
            "The specified users are not friend.");

        public static Error DateAndTimeIsInThePast => new(
            "GroupEvent.InThePast",
            "The event date and time cannot be in the past.");
    }

    /// <summary>
    /// Contains the personal event errors.
    /// </summary>
    public static class PersonalEvent
    {
        public static Error NotFound => new(
            "GroupEvent.NotFound",
            "The group event with the specified identifier was not found.");

        public static Error UserNotFound => new(
            "GroupEvent.UserNotFound",
            "The user with the specified identifier was not found.");
            
        public static Error DateAndTimeIsInThePast => new(
            "GroupEvent.InThePast",
            "The event date and time cannot be in the past.");

        public static Error AlreadyProcessed =>
            new("PersonalEvent.AlreadyProcessed", "The event has already been processed.");
    }

    /// <summary>
    /// Contains the notification errors.
    /// </summary>
    public static class Notification
    {
        public static Error AlreadySent => new("Notification.AlreadySent", "The notification has already been sent.");
    }

    /// <summary>
    /// Contains the invitation errors.
    /// </summary>
    public static class Invitation
    {
        public static Error NotFound => new(
            "Invitation.NotFound",
            "The invitation with the specified identifier was not found.");

        public static Error EventNotFound => new(
            "Invitation.EventNotFound",
            "The event with the specified identifier was not found.");

        public static Error UserNotFound => new(
            "Invitation.UserNotFound",
            "The user with the specified identifier was not found.");

        public static Error FriendNotFound => new(
            "Invitation.FriendNotFound",
            "The friend with the specified identifier was not found.");

        public static Error AlreadyAccepted => new("Invitation.AlreadyAccepted", "The invitation has already been accepted.");

        public static Error AlreadyRejected => new("Invitation.AlreadyRejected", "The invitation has already been rejected.");
    }

    /// <summary>
    /// Contains the friendship errors.
    /// </summary>
    public static class Friendship
    {
        public static Error UserNotFound => new(
            "Friendship.UserNotFound",
            "The user with the specified identifier was not found.");

        public static Error FriendNotFound => new(
            "Friendship.FriendNotFound",
            "The friend with the specified identifier was not found.");

        public static Error NotFriends => new(
            "Friendship.NotFriends",
            "The specified users are not friend.");
    }

    /// <summary>
    /// Contains the friendship request errors.
    /// </summary>
    public static class FriendshipRequest
    {
        public static Error NotFound => new(
            "FriendshipRequest.NotFound",
            "The friendship request with the specified identifier was not found.");

        public static Error UserNotFound => new(
            "FriendshipRequest.UserNotFound",
            "The user with the specified identifier was not found.");

        public static Error FriendNotFound => new(
            "FriendshipRequest.FriendNotFound",
            "The friend with the specified identifier was not found.");

        public static Error AlreadyAccepted => new(
            "FriendshipRequest.AlreadyAccepted",
            "The friendship request has already been accepted.");

        public static Error AlreadyRejected => new(
            "FriendshipRequest.AlreadyRejected",
            "The friendship request has already been rejected.");

        public static Error AlreadyFriends => new(
            "FriendshipRequest.AlreadyFriends",
            "The friendship request can not be sent because the users are already friends.");

        public static Error PendingFriendshipRequest => new(
            "FriendshipRequest.PendingFriendshipRequest",
            "The friendship request can not be sent because there is a pending friendship request.");
    }

    /// <summary>
    /// Contains the name errors.
    /// </summary>
    public static class Name
    {
        public static Error NullOrEmpty => new("Name.NullOrEmpty", "The name is required.");

        public static Error LongerThanAllowed => new("Name.LongerThanAllowed", "The name is longer than allowed.");
    }

    /// <summary>
    /// Contains the first name errors.
    /// </summary>
    public static class FirstName
    {
        public static Error NullOrEmpty => new("FirstName.NullOrEmpty", "The first name is required.");

        public static Error LongerThanAllowed => new("FirstName.LongerThanAllowed", "The first name is longer than allowed.");
    }

    /// <summary>
    /// Contains the last name errors.
    /// </summary>
    public static class LastName
    {
        public static Error NullOrEmpty => new("LastName.NullOrEmpty", "The last name is required.");

        public static Error LongerThanAllowed => new("LastName.LongerThanAllowed", "The last name is longer than allowed.");
    }

    /// <summary>
    /// Contains the emailAddress errors.
    /// </summary>
    public static class Email
    {
        public static Error NullOrEmpty => new("EmailAddress.NullOrEmpty", "The emailAddress is required.");

        public static Error LongerThanAllowed => new("EmailAddress.LongerThanAllowed", "The emailAddress is longer than allowed.");

        public static Error InvalidFormat => new("EmailAddress.InvalidFormat", "The emailAddress format is invalid.");
    }

    /// <summary>
    /// Contains the password errors.
    /// </summary>
    public static class Password
    {
        public static Error NullOrEmpty => new("Password.NullOrEmpty", "The password is required.");

        public static Error TooShort => new("Password.TooShort", "The password is too short.");

        public static Error MissingUppercaseLetter => new(
            "Password.MissingUppercaseLetter",
            "The password requires at least one uppercase letter.");

        public static Error MissingLowercaseLetter => new(
            "Password.MissingLowercaseLetter",
            "The password requires at least one lowercase letter.");

        public static Error MissingDigit => new(
            "Password.MissingDigit",
            "The password requires at least one digit.");

        public static Error MissingNonAlphaNumeric => new(
            "Password.MissingNonAlphaNumeric",
            "The password requires at least one non-alphanumeric.");
    }

    /// <summary>
    /// Contains general errors.
    /// </summary>
    public static class General
    {
        public static Error UnProcessableRequest => new(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        public static Error ServerError => new("General.ServerError", "The server encountered an unrecoverable error.");
    }

    /// <summary>
    /// Contains the authentication errors.
    /// </summary>
    public static class Authentication
    {
        public static Error InvalidEmailOrPassword => new(
            "Authentication.InvalidEmailOrPassword",
            "The specified emailAddress or password are incorrect.");
    }
}