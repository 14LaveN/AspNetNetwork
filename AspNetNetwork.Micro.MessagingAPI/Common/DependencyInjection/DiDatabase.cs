using AspNetNetwork.Application.Core.Settings;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Database.Message;
using AspNetNetwork.Database.MetricsAndMessages;

namespace AspNetNetwork.Micro.MessagingAPI.Common.DependencyInjection;

public static class DiDatabase
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMongoDatabase(configuration);
        services.AddUserDatabase(configuration);
        services.AddMessagesDatabase(configuration);
        
        services.Configure<MongoSettings>(
            configuration.GetSection(MongoSettings.MongoSettingsKey));
        
        return services;
    }
}