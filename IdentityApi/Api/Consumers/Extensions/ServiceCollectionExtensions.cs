using Core.RabbitLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.Consumers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumers(this IServiceCollection services)
    {
        services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConsumer, UserInfoConsumer>());
        services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConsumer, UserInfoListConsumer>());
        return services;
    }
}