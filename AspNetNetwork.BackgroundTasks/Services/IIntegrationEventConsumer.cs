using AspNetNetwork.Application.Core.Abstractions.Messaging;

namespace AspNetNetwork.BackgroundTasks.Services;

/// <summary>
/// Represents the integration event consumer interface.
/// </summary>
internal interface IIntegrationEventConsumer
{
    /// <summary>
    /// Consumes the incoming the specified integration event.
    /// </summary>
    Task Consume(IIntegrationEvent? integrationEvent);
}