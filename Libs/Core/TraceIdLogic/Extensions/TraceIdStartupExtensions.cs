using Core.TraceIdLogic.Interfaces;
using Core.TraceLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.TraceIdLogic.Extensions;

public static class TraceIdStartupExtensions
{
    public static IServiceCollection TryAddTraceId(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<TraceIdAccessor>();
        serviceCollection.TryAddEnumerable(ServiceDescriptor
            .Scoped<ITraceWriter, TraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>()));
        serviceCollection.TryAddEnumerable(ServiceDescriptor
            .Scoped<ITraceReader, TraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>()));;
        serviceCollection.TryAddEnumerable(ServiceDescriptor
            .Scoped<ITraceIdAccessor, TraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>()));

        return serviceCollection;
    }
}