using AspNetNetwork.Application.Core.Abstractions.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AspNetNetwork.Email.Emails;
using AspNetNetwork.Email.Emails.Settings;

namespace AspNetNetwork.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
            throw new ArgumentException();
    
        // services.ConfigureOptions<MailSettings>(configuration.GetSection(MailSettings.SettingsKey));
        
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        
        return services;
    }
}