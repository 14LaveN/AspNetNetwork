using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Message.Events;
using Newtonsoft.Json;

namespace AspNetNetwork.RabbitMq.Messaging.Messages.Events.Create;

/// <summary>
/// Represents the integration event that is raised when a message is created.
/// </summary>
public sealed class MessageCreatedIntegrationEvent
    : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageCreatedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="messageCreatedDomainEvent">The message created domain event.</param>
    internal MessageCreatedIntegrationEvent(MessageCreatedDomainEvent messageCreatedDomainEvent) => MessageId = messageCreatedDomainEvent.Message.Id;
        
    [JsonConstructor]
    private MessageCreatedIntegrationEvent(Guid messageId) => MessageId = messageId;

    /// <summary>
    /// Gets the message identifier.
    /// </summary>
    public Guid MessageId { get; }
}