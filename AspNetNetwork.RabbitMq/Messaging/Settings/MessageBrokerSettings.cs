using System.ComponentModel.DataAnnotations;

namespace AspNetNetwork.RabbitMq.Messaging.Settings;

/// <summary>
/// Represents the message broker settings.
/// </summary>
public sealed class MessageBrokerSettings
{
    public const string SettingsKey = "MessageBroker";

    /// <summary>
    /// Gets or sets the host name.
    /// </summary>
    [Required, Url]
    public static string? AmqpLink { get; set; }

    /// <summary>
    /// Gets or sets the queue name.
    /// </summary>
    [Required]
    public static string? QueueName { get; set; }
}