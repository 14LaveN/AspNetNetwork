using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.PersonalEvent.Data.Interfaces;
using AspNetNetwork.Database.PersonalEvent.Data.Repositories;

namespace AspNetNetwork.Database.PersonalEvent;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddPersonalEventDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IPersonalEventRepository, PersonalEventRepository>();
        services.AddScoped<IUnitOfWork<Domain.Identity.Entities.PersonalEvent>, UnitOfWork<Domain.Identity.Entities.PersonalEvent>>();

        return services;
    }
}