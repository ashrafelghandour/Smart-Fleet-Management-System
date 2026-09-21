
using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FleetManagementSystem.Application.LoggingBehavior;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
   

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Handling {RequestName} started", requestName);

        try
        {
            var response = await next();
            stopwatch.Stop();
            
            logger.LogInformation("Handling {RequestName} completed in {ElapsedMilliseconds}ms", 
                requestName, stopwatch.ElapsedMilliseconds);
            
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling {RequestName}", requestName);
            throw;
        }
    }
}