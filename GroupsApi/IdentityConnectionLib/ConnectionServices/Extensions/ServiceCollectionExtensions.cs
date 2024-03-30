using IdentityConnectionLib.ConnectionServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib.ConnectionServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityConnectionService(this IServiceCollection services)
    {
        services.AddScoped<IIdentityConnectionService, IdentityConnectionService>();
        return services;
    }
}