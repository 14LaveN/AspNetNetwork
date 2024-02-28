using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Message.Data.Interfaces;
using AspNetNetwork.Database.Message.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetNetwork.Database.Message;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMessagesDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString("PAGenericDb");
        
        services.AddDbContext<MessageDbContext>(o => 
            o.UseNpgsql(connectionString, act 
                    =>
                {
                    act.EnableRetryOnFailure(3);
                    act.CommandTimeout(30);
                })
                .LogTo(Console.WriteLine)
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
        
        if (connectionString is not null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString);
        
        services.AddScoped<IMessagesRepository, MessagesRepository>();
        services.AddScoped<BaseDbContext<Domain.Identity.Entities.Message>, MessageDbContext>();
        services.AddScoped<IUnitOfWork<Domain.Identity.Entities.Message>, UnitOfWork<Domain.Identity.Entities.Message>>();

        return services;
    }
}