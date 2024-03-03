using AspNetNetwork.Application.Core.Behaviours;
using AspNetNetwork.Micro.MessagingAPI.Mediatr.Behaviors;
using AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.CreateMessage;
using AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.UpdateMessage;
using MediatR;
using MediatR.NotificationPublishers;

namespace AspNetNetwork.Micro.MessagingAPI.Common.DependencyInjection;

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

            x.RegisterServicesFromAssemblies(typeof(CreateMessageCommand).Assembly,
                typeof(CreateMessageCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(UpdateMessageCommand).Assembly,
                typeof(UpdateMessageCommandHandler).Assembly);
            
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(MessageTransactionBehavior<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            x.NotificationPublisher = new ForeachAwaitPublisher();
        });
        
        return services;
    }
}