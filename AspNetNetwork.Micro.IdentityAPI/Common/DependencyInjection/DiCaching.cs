namespace AspNetNetwork.Micro.IdentityAPI.Common.DependencyInjection;

public static class DiCaching
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddResponseCaching(options =>
        {
            options.UseCaseSensitivePaths = false; 
            options.MaximumBodySize = 1024; 
        });
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "Identity_";
        });

        services.AddDistributedMemoryCache();
        
        return services;
    }
}