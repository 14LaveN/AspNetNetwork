using System.Text;
using System.Text.Json;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.RabbitMq.Messaging.Settings;
using RabbitMQ.Client;

namespace AspNetNetwork.RabbitMq.Messaging;

/// <summary>
/// Represents the integration event publisher.
/// </summary>
public sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    /// <summary>
    /// Initialize connection.
    /// </summary>
    /// <returns>Returns connection to RabbitMQ.</returns> 
    private static async Task<IConnection> CreateConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri("amqps://dgpswpjt:tbQvnOh93n-sdqDMjXAjfB53OiShmOka@chimpanzee.rmq.cloudamqp.com/dgpswpjt")
        };

        var connection = await connectionFactory.CreateConnectionAsync();

        return connection;
    }
    
    /// <summary>
    /// Initialize channel.
    /// </summary>
    /// <returns>Returns channel for RabbitMQ.</returns>
    private static async Task<IChannel> CreateChannel()
    {
        var connection = await CreateConnection();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(MessageBrokerSettings.QueueName, false, false, false);
        await channel.QueueBindAsync(MessageBrokerSettings.QueueName,
            exchange: MessageBrokerSettings.QueueName + "Exchange",
            routingKey: MessageBrokerSettings.QueueName);

        return channel;
    }

    /// <inheritdoc />
    public async Task Publish(IIntegrationEvent integrationEvent)
    {
        var channel = await CreateChannel();
        string payload = JsonSerializer.Serialize(integrationEvent, typeof(IIntegrationEvent));

        var body = Encoding.UTF8.GetBytes(payload);

        if (MessageBrokerSettings.QueueName is not null)
            await channel.BasicPublishAsync(MessageBrokerSettings.QueueName + "Exchange",
                MessageBrokerSettings.QueueName, body: body);

        await channel.CloseAsync();
    }
}