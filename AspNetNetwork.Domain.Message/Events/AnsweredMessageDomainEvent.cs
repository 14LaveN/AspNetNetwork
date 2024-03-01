using AspNetNetwork.Domain.Common.Core.Events;

namespace AspNetNetwork.Domain.Message.Events;

/// <summary>
/// Represents the event that is raised when a message is answered.
/// </summary>
public sealed class AnsweredMessageDomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnsweredMessageDomainEvent"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public AnsweredMessageDomainEvent(Identity.Entities.Message message) => Message = message;

    /// <summary>
    /// Gets the message.
    /// </summary>
    public Identity.Entities.Message Message { get; }
}