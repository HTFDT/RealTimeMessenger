using Core.TraceIdLogic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Middleware;

/// <summary>
/// Миддлварь для того, чтобы добавлять значение TraceId, если его еще нет.
/// Регистрирутеся в каждом сервисе строго после TraceWriterMiddleware
/// </summary>
/// <param name="traceIdAccessor"></param>
public class TraceIdMiddleware(ITraceIdAccessor traceIdAccessor) : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (traceIdAccessor.ReadValue() is null)
            traceIdAccessor.WriteValue(Guid.NewGuid().ToString());
        return next(context);
    }
}