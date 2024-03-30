using System.Text;
using Core.Jwt.Interfaces;
using Core.TraceLogic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Core.Jwt.Extensions;

public static class CustomJwtAuthExtensions
{
    public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        return services;
    }

    public static IServiceCollection AddCustomAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Superuser", policy => policy.RequireRole("Superuser"));
            options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator", "Superuser"));
        });
        return services;
    }
    
    public static IServiceCollection TryAddTraceJwtToken(this IServiceCollection services)
    {
        services.AddScoped<JwtTokenAccessor>();
        services.TryAddEnumerable(ServiceDescriptor
            .Scoped<ITraceWriter, JwtTokenAccessor>(provider => provider.GetRequiredService<JwtTokenAccessor>()));
        services.TryAddEnumerable(ServiceDescriptor
            .Scoped<ITraceReader, JwtTokenAccessor>(provider => provider.GetRequiredService<JwtTokenAccessor>()));
        services.TryAddEnumerable(ServiceDescriptor
            .Scoped<IJwtTokenAccessor, JwtTokenAccessor>(provider => provider.GetRequiredService<JwtTokenAccessor>()));
        return services;
    }
}