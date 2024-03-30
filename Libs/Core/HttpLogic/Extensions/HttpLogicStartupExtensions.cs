using Core.HttpLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.HttpLogic.Extensions;

/// <summary>
/// Регистрация в DI сервисов для HTTP-соединений
/// </summary>
public static class HttpLogicStartupExtensions
{
    /// <summary>
    /// Добавление сервиса для осуществления запросов по HTTP
    /// </summary>
    public static IServiceCollection AddHttpRequestService(this IServiceCollection services)
    {
        services
            .AddHttpContextAccessor()
            .AddHttpClient()
            .AddTransient<IHttpConnectionService, HttpConnectionService>();
        
        services.TryAddTransient<IHttpRequestService, HttpRequestService>();
        
        return services;
    }
}