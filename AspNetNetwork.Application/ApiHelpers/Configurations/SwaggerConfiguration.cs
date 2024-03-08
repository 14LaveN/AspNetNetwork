using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AspNetNetwork.Application.ApiHelpers.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwachbackleService(
        this IServiceCollection services,
        Assembly assembly,
        string apiName)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        return services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "Поколение 1",
                Title = apiName,
                Description = $"Backend Web API на C# .NET for TeamTasks.Micro.{apiName} application",
                Contact = new OpenApiContact
                {
                    Name = "GitHub",
                    Url = new Uri("https://github.com/14LaveN")
                }
            });

            if (apiName is not "IdentityAPI")
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header используется в расшифровке запроса.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            }

            var xmlFilename = $"{assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public static IApplicationBuilder UseSwaggerApp(this IApplicationBuilder app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            options.RoutePrefix = string.Empty;
        });
        return app;
    }
}