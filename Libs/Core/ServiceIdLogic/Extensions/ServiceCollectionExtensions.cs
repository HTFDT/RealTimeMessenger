using Core.ServiceIdLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ServiceIdLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceIdAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IServiceIdAccessor, ServiceIdAccessor>();
        return services;
    }
}