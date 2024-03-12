using Services.Implementations;
using Services.Interfaces.Interfaces;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IServicesManager, ServicesManager>();
        return services;
    }
}