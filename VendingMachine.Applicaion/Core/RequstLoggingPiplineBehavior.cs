using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Applicaion.Core;

public sealed class RequstLoggingPiplineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly ILogger<RequstLoggingPiplineBehavior<TRequest,TResponse>> _logger;

    public RequstLoggingPiplineBehavior(ILogger<RequstLoggingPiplineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requstName = typeof(TRequest).Name;
        _logger.LogInformation("Processing requst {RequstName}", requstName);
        TResponse result = await next();
        if(result.IsSuccess)
        {
            _logger.LogInformation("Completed requst {RequstName}", requstName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Errors, true))
            {

                _logger.LogError("Completed requst {RequstName} with error", requstName);
            }
        }
        return result;
        
    }
}
