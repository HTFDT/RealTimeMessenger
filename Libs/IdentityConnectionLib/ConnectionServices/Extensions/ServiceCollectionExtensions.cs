using IdentityConnectionLib.ConnectionServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib.ConnectionServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityConnectionService(this IServiceCollection services, IConfiguration config)
    {
        switch (config["ServiceInteraction:Type"])
        {
            case "http":
                services.AddScoped<IIdentityConnectionService, HttpIdentityConnectionService>();
                break;
            case "broker":
                services.AddSingleton<IIdentityConnectionService, RabbitMqIdentityConnectionService>();
                break;
            default:
                services.AddScoped<IIdentityConnectionService, HttpIdentityConnectionService>();
                break;
        }
            
        return services;
    }
}