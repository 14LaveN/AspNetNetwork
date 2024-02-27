using AspNetNetwork.Application.Core.Helpers.Metric;

namespace AspNetNetwork.Micro.IdentityAPI.Common.DependencyInjection;

public static class DiHelpers
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<CreateMetricsHelper>();
        
        return services;
    }
}