using System.Reflection;
using Microsoft.Extensions.Configuration;
using AspNetNetwork.BackgroundTasks.QuartZ;
using AspNetNetwork.BackgroundTasks.QuartZ.Jobs;
using AspNetNetwork.BackgroundTasks.QuartZ.Schedulers;
using AspNetNetwork.BackgroundTasks.Services;
using AspNetNetwork.BackgroundTasks.Settings;
using AspNetNetwork.BackgroundTasks.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz.Spi;

namespace AspNetNetwork.BackgroundTasks;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<BackgroundTaskSettings>(configuration.GetSection(BackgroundTaskSettings.SettingsKey));

        services.AddHostedService<GroupEventNotificationsProducerBackgroundService>();

        services.AddHostedService<PersonalEventNotificationsProducerBackgroundService>();

        services.AddHostedService<EmailNotificationConsumerBackgroundService>();

        services.AddHostedService<IntegrationEventConsumerBackgroundService>();


        services.AddScoped<IPersonalEventNotificationsProducer, PersonalEventNotificationsProducer>();

        services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();

        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        services.AddTransient<IJobFactory, QuartzJobFactory>();
        services.AddSingleton(_ =>
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            
            return scheduler;
        });
        services.AddTransient<AbstractScheduler<UserDbJob>>();
        services.AddTransient<SaveMetricsScheduler>();
        
        return services;
    }
}