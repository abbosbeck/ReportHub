using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(
                "Exception occurred {@Exception}, {@DateTimeUtc}",
                exception,
                DateTime.UtcNow);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error",
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Type = "https://httpstatuses.com/500"
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response
                .WriteAsJsonAsync(problemDetails);
        }
    }
}
