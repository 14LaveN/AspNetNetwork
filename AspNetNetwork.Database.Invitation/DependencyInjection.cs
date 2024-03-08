using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Invitation.Data.Interfaces;
using AspNetNetwork.Database.Invitation.Data.Repositories;

namespace AspNetNetwork.Database.Invitation;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddInvitationsDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IUnitOfWork<Domain.Identity.Entities.Invitation>, UnitOfWork<Domain.Identity.Entities.Invitation>>();

        return services;
    }
}