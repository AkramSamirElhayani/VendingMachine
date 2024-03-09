using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace VendingMachine.Api.Infrastructer;

public class RequstLogMiddleware
{
 
    private readonly RequestDelegate _next;

    public RequstLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public  Task InvokeAsync(HttpContext context)
    {
       
        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        {
            return _next.Invoke(context);
        }
    }

  
}
