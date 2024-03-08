using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Notification.Data.Interfaces;
using AspNetNetwork.Database.Notification.Data.Repositories;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.Notification;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddNotificationsDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<BaseDbContext>();
        
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUnitOfWork<Domain.Identity.Entities.Notification>, UnitOfWork<Domain.Identity.Entities.Notification>>();

        return services;
    }
}