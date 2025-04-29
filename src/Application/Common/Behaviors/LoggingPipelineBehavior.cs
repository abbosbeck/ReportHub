using Application.Common.Interfaces.Time;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger,
    IDateTimeService dateTimeService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IBaseRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        logger.LogInformation(
            "Starting request {@RequestName}, {@DateTimeUtc}",
            requestName,
            dateTimeService.UtcNow);

        var result = await next();

        logger.LogInformation(
            "Completed request {@RequestName}, {@DateTimeUtc}",
            requestName,
            dateTimeService.UtcNow);

        return result;
    }
}
