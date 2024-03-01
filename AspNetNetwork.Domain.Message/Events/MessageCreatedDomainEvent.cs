using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Message.Events;

/// <summary>
/// Represents the event that is raised when a message is created.
/// </summary>
public sealed class MessageCreatedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public MessageCreatedDomainEvent(Identity.Entities.Message message) => Message = message;

    /// <summary>
    /// Gets the message.
    /// </summary>
    public Identity.Entities.Message Message { get; }
}