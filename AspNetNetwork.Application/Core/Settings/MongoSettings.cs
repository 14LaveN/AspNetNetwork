namespace AspNetNetwork.Application.Core.Settings;

/// <summary>
/// Represents the mongo settings class.
/// </summary>
public sealed class MongoSettings
{
    /// <summary>
    /// Gets mongo settings key.
    /// </summary>
    public static string MongoSettingsKey = "MongoConnection";
    
    /// <summary>
    /// Gets or sets connection string.
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Gets or sets database name.
    /// </summary>
    public string Database { get; set; } = null!;

    /// <summary>
    /// Gets or sets Rabbit Messages Collection Name.
    /// </summary>
    public string RabbitMessagesCollectionName { get; set; } = null!;

    /// <summary>
    /// Gets or sets Metrics Collection Name.
    /// </summary>
    public string MetricsCollectionName { get; set; } = null!;
}