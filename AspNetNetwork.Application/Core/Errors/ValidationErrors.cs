using AspNetNetwork.Domain.Common.Core.Primitives;

namespace AspNetNetwork.Application.Core.Errors;

/// <summary>
/// Contains the validation errors.
/// </summary>
public static class ValidationErrors
{
    /// <summary>
    /// Contains the login errors.
    /// </summary>
    internal static class Login
    {
        internal static Error EmailIsRequired => new Error("Login.EmailIsRequired", "The emailAddress is required.");

        internal static Error PasswordIsRequired => new Error("Login.PasswordIsRequired", "The password is required.");
    }
    
    /// <summary>
    /// Contains the create message errors.
    /// </summary>
    public static class CreateMessage
    {
        public static Error DescriptionIsRequired => 
            new("CreateMessage.DescriptionIsRequired", "The description is required.");

        public static Error RecipientIdIsRequired => 
            new("CreateMessage.RecipientIdIsRequired", "The recipient identifier is required.");
    }

    /// <summary>
    /// Contains the create group event errors.
    /// </summary>
    public static class CreateGroupEvent
    {
        public static Error UserIdIsRequired => new Error("CreateGroupEvent.UserIdIsRequired", "The user identifier is required.");

        public static Error NameIsRequired => new Error("CreateGroupEvent.NameIsRequired", "The event name is required.");

        public static Error CategoryIdIsRequired => new Error(
            "CreateGroupEvent.CategoryIdIsRequired",
            "The category identifier is required.");

        public static Error DateAndTimeIsRequired => new Error(
            "CreateGroupEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }

    /// <summary>
    /// Contains the update group event errors.
    /// </summary>
    public static class UpdateGroupEvent
    {
        public static Error GroupEventIdIsRequired => new Error(
            "UpdateGroupEvent.GroupEventIdIsRequired",
            "The group event identifier is required.");

        public static Error NameIsRequired => new Error("UpdateGroupEvent.NameIsRequired", "The event name is required.");

        public static Error DateAndTimeIsRequired => new Error(
            "UpdateGroupEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }

    /// <summary>
    /// Contains the cancel group event errors.
    /// </summary>
    public static class CancelGroupEvent
    {
        public static Error GroupEventIdIsRequired => new Error(
            "CancelGroupEvent.GroupEventIdIsRequired",
            "The group event identifier is required.");
    }

    /// <summary>
    /// Contains the invite friend to group event errors.
    /// </summary>
    public static class InviteFriendToGroupEvent
    {
        public static Error GroupEventIdIsRequired => new Error(
            "InviteFriendToGroupEvent.GroupEventIdIsRequired",
            "The group event identifier is required.");

        public static Error FriendIdIsRequired => new Error(
            "InviteFriendToGroupEvent.FriendIdIsRequired",
            "The friend identifier is required.");
    }

    /// <summary>
    /// Contains the accept invitation errors.
    /// </summary>
    public static class AcceptInvitation
    {
        public static Error InvitationIdIsRequired => new Error(
            "AcceptInvitation.InvitationIdIsRequired",
            "The invitation identifier is required.");
    }

    /// <summary>
    /// Contains the reject invitation errors.
    /// </summary>
    public static class RejectInvitation
    {
        public static Error InvitationIdIsRequired => new Error(
            "RejectInvitation.InvitationIdIsRequired",
            "The invitation identifier is required.");
    }

    /// <summary>
    /// Contains the create personal event errors.
    /// </summary>
    public static class CreatePersonalEvent
    {
        public static Error UserIdIsRequired => new Error(
            "CreatePersonalEvent.UserIdIsRequired",
            "The user identifier is required.");

        public static Error NameIsRequired => new Error("CreatePersonalEvent.NameIsRequired", "The event name is required.");

        public static Error CategoryIdIsRequired => new Error(
            "CreatePersonalEvent.CategoryIdIsRequired",
            "The category identifier is required.");

        public static Error DateAndTimeIsRequired => new Error(
            "CreatePersonalEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }

    /// <summary>
    /// Contains the update personal event errors.
    /// </summary>
    public static class UpdatePersonalEvent
    {
        public static Error GroupEventIdIsRequired => new Error(
            "UpdatePersonalEvent.GroupEventIdIsRequired",
            "The group event identifier is required.");

        public static Error NameIsRequired => new Error("UpdatePersonalEvent.NameIsRequired", "The event name is required.");

        public static Error DateAndTimeIsRequired => new Error(
            "UpdatePersonalEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }

    /// <summary>
    /// Contains the cancel personal event errors.
    /// </summary>
    public static class CancelPersonalEvent
    {
        public static Error PersonalEventIdIsRequired => new Error(
            "CancelPersonalEvent.GroupEventIdIsRequired",
            "The group event identifier is required.");
    }

    /// <summary>
    /// Contains the change password errors.
    /// </summary>
    public static class ChangePassword
    {
        public static Error UserIdIsRequired => new Error("ChangePassword.UserIdIsRequired", "The user identifier is required.");

        public static Error PasswordIsRequired => new Error("ChangePassword.PasswordIsRequired", "The password is required.");
    }

    /// <summary>
    /// Contains the create user errors.
    /// </summary>
    internal static class CreateUser
    {
        internal static Error FirstNameIsRequired => new Error("CreateUser.FirstNameIsRequired", "The first name is required.");

        internal static Error LastNameIsRequired => new Error("CreateUser.LastNameIsRequired", "The last name is required.");

        internal static Error EmailIsRequired => new Error("CreateUser.EmailIsRequired", "The emailAddress is required.");

        internal static Error PasswordIsRequired => new Error("CreateUser.PasswordIsRequired", "The password is required.");
    }
        
    /// <summary>
    /// Contains the send friendship request errors.
    /// </summary>
    internal static class SendFriendshipRequest
    {
        internal static Error UserIdIsRequired => new Error(
            "SendFriendshipRequest.UserIdIsRequired",
            "The user identifier is required.");

        internal static Error FriendIdIsRequired => new Error(
            "SendFriendshipRequest.FriendIdIsRequired",
            "The friend identifier is required.");
    }

    /// <summary>
    /// Contains the update user errors.
    /// </summary>
    internal static class UpdateUser
    {
        internal static Error UserIdIsRequired => new Error("UpdateUser.UserIdIsRequired", "The user identifier is required.");

        internal static Error FirstNameIsRequired => new Error("UpdateUser.FirstNameIsRequired", "The first name is required.");

        internal static Error LastNameIsRequired => new Error("UpdateUser.LastNameIsRequired", "The last name is required.");
    }
}