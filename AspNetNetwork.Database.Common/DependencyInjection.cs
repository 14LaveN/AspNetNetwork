using AspNetNetwork.Database.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetNetwork.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspNetNetwork.Database.Common;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBaseDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var connectionString = configuration.GetConnectionString("ANGenericDb");

        if (connectionString is not null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString);
        
        services.AddDbContext<BaseDbContext>(o =>
            o.UseNpgsql(connectionString, act
                    =>
            {
                act.EnableRetryOnFailure(3);
                act.CommandTimeout(30);
            })
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables))
                .LogTo(Console.WriteLine)
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        return services;
    }
}