using AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.CreateMessage;
using AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.UpdateMessage;
using FluentValidation;

namespace AspNetNetwork.Micro.MessagingAPI.Common.DependencyInjection;

public static class DiValidator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IValidator<CreateMessageCommand>, CreateMessageCommandValidator>();
        services.AddScoped<IValidator<UpdateMessageCommand>, UpdateMessageCommandValidator>();
        
        return services;
    }
}