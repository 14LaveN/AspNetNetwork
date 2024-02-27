using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Application.Common;
using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Application.Core.Abstractions.Helpers.JWT;
using AspNetNetwork.Application.Core.Helpers.JWT;
using AspNetNetwork.Application.Core.Helpers.Metric;
using AspNetNetwork.Email.Emails;
using AspNetNetwork.Email.Emails.Settings;

namespace AspNetNetwork.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentException();

        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddScoped<CreateMetricsHelper>();
        services.AddScoped<IDateTime, MachineDateTime>();
        
        return services;
    }
}