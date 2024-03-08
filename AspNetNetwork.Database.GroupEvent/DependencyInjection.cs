using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Common.Interceptors;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Database.GroupEvent.Data.Repositories;

namespace AspNetNetwork.Database.GroupEvent;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddGroupEventDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IGroupEventRepository, GroupEventRepository>();
        services.AddScoped<IUnitOfWork<Domain.Identity.Entities.GroupEvent>, UnitOfWork<Domain.Identity.Entities.GroupEvent>>();

        return services;
    }
}