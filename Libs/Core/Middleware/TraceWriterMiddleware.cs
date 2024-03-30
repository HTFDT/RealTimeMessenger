using Core.TraceLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.Middleware;

public sealed class TraceWriterMiddleware(IEnumerable<ITraceWriter> traceWriters) : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var writer in traceWriters)
            if (context.Request.Headers.TryGetValue(writer.Name, out var values))
                writer.WriteValue(string.Join(";", values.ToArray()));
            
        return next(context);
    }
}