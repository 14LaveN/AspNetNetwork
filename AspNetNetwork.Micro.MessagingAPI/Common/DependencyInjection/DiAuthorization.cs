using System.Text;
using System.Text.Json;
using AspNetNetwork.Application.Core.Settings.User;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Domain.Identity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AspNetNetwork.Micro.MessagingAPI.Common.DependencyInjection;

public static class DiAuthorization
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddAuthorizationExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = new SymmetricSecurityKey("secretsecret123456"u8.ToArray());

                options.TokenValidationParameters.ValidIssuer = "https://localhost:7006/";
                options.TokenValidationParameters.ValidAudiences
                    = new List<string>()
                        { "https://localhost:7006/", "https://localhost:7093/" };
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
            });

        services.AddAuthorization();

        services.AddCors(options => options.AddDefaultPolicy(corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("https://localhost:44460/", "http://localhost:44460/", "http://localhost:44460")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));
        
        return services;
    }
}