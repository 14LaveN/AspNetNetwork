

using AspNetNetwork.Domain.Common.Core.Events;
using AspNetNetwork.Domain.Identity.Events.Invitation;
using AspNetNetwork.RabbitMq.Messaging;

namespace AspNetNetwork.Events.Invitation.Events.InvitationSent;

/// <summary>
/// Represents the <see cref="InvitationRejectedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnInvitationSentDomainEventHandler : IDomainEventHandler<InvitationSentDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnInvitationSentDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnInvitationSentDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(InvitationSentDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new InvitationSentIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}