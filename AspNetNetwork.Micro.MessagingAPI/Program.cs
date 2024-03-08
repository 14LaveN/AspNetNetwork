using System.Reflection;
using App.Metrics.Formatters.Prometheus;
using AspNetNetwork.Application;
using AspNetNetwork.Application.ApiHelpers.Configurations;
using AspNetNetwork.Application.ApiHelpers.Middlewares;
using AspNetNetwork.BackgroundTasks;
using AspNetNetwork.BackgroundTasks.QuartZ.Schedulers;
using AspNetNetwork.Email;
using AspNetNetwork.Micro.MessagingAPI.Common.DependencyInjection;
using AspNetNetwork.RabbitMq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.HttpRequestDurations;

#region BuilderRegion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Host.UseMetricsWebTracking(options => 
        options.OAuth2TrackingEnabled = true)
    .UseMetricsEndpoints(options =>
    {
        options.EnvironmentInfoEndpointEnabled = true;
        options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        options.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
    });

builder.Services.AddControllers();

builder.Services.AddValidators();

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddBackgroundTasks(builder.Configuration);

builder.Services.AddRabbitBackgroundTasks();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddMediatr();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddHelpers();

builder.Services.AddSwachbackleService(Assembly.GetExecutingAssembly(), "MessagingAPI");

builder.Services.AddCaching();

builder.Services.AddApplication();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthorizationExtension(builder.Configuration);

#endregion

#region ApplicationRegion

var app = builder.Build();

var scheduler = new MessageDbScheduler();
scheduler.Start(builder.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerApp();
}

app.UseCors();

UseMetrics();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.MapControllers();

UseCustomMiddlewares();

app.Run();
return;

#endregion

#region UseMiddlewaresRegion

void UseCustomMiddlewares()
{
    if (app is null)
        throw new ArgumentException();

    app.UseMiddleware<RequestLoggingMiddleware>(app.Logger);
    app.UseMiddleware<ResponseCachingMiddleware>();
}

void UseMetrics()
{
    if (app is null)
        throw new ArgumentException();
    
    app.UseMetricServer();
    app.UseHttpMetrics();
    app.UsePrometheusServer();
    app.UsePrometheusRequestDurations();
}

#endregion
