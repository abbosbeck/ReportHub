using Application.Common.Interfaces.Time;
using Application.Common.Services;
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
        LogSenderService.SendToTelegram("8147917610:AAFHvcCOt7ozmT7Ib00Pj_Ku0qbatcCaoBk", "-1002601174074", $"Starting request: {requestName}");

        var result = await next();

        logger.LogInformation(
            "Completed request {@RequestName}, {@DateTimeUtc}",
            requestName,
            dateTimeService.UtcNow);
        LogSenderService.SendToTelegram("8147917610:AAFHvcCOt7ozmT7Ib00Pj_Ku0qbatcCaoBk", "-1002601174074", $"Completed request: {requestName}");

        return result;
    }
}
