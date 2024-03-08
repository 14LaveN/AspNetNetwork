using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Database.Identity.Data.Repositories;

namespace AspNetNetwork.Database.Identity;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddUserDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}