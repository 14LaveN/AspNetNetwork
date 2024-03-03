using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Message.Events;

namespace AspNetNetwork.RabbitMq.Messaging.Messages.Events.Create;

/// <summary>
/// Represents the <see cref="MessageCreatedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnMessageCreatedDomainEventHandler
    : IDomainEventHandler<MessageCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnMessageCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnMessageCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(MessageCreatedDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new MessageCreatedIntegrationEvent(notification));
}