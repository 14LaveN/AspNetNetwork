using AspNetNetwork.Application.Core.Behaviours;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.ChangePassword;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Login;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Register;
using MediatR;
using MediatR.NotificationPublishers;

namespace AspNetNetwork.Micro.IdentityAPI.Common.DependencyInjection;

public static class DiMediator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly,
                typeof(RegisterCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly,
                typeof(LoginCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangePasswordCommand).Assembly,
                typeof(ChangePasswordCommandHandler).Assembly);
            
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UserTransactionBehaviour<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            x.NotificationPublisher = new ForeachAwaitPublisher();
        });
        
        return services;
    }
}