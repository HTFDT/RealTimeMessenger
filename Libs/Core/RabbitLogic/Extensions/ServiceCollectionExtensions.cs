using Core.RabbitLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.RabbitLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitClientServices(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqClientService, RabbitMqClientService>();
        return services;
    }
}