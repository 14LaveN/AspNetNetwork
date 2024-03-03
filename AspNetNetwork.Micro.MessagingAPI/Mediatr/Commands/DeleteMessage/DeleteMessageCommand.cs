namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.DeleteMessage;

/// <summary>
/// Represents the delete message command record class.
/// </summary>
/// <param name="MessageId">The message identifier.</param>
public sealed record DeleteMessageCommand(Guid MessageId);