using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var problemDetails = exception switch
            {
                ValidationException validationException =>
                    new ProblemDetails()
                    {
                        Title = nameof(validationException),
                        Detail = validationException.Message,
                        Type = "https://httpstatuses.com/400",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = context.Request.Path,
                        Extensions =
                        {
                            ["errors"] = validationException.Errors,
                        },
                    },
                BadRequestException =>
                    new ProblemDetails
                    {
                        Title = nameof(exception),
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/400",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = context.Request.Path,
                    },
                UnauthorizedException =>
                    new ProblemDetails
                    {
                        Title = nameof(exception),
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/401",
                        Status = StatusCodes.Status401Unauthorized,
                        Instance = context.Request.Path,
                    },
                ForbiddenException =>
                    new ProblemDetails
                    {
                        Title = nameof(exception),
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/403",
                        Status = StatusCodes.Status403Forbidden,
                        Instance = context.Request.Path,
                    },
                _ => new ProblemDetails
                {
                    Title = nameof(exception),
                    Detail = exception.Message,
                    Type = "https://httpstatuses.com/500",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path,
                }
            };

            context.Response.StatusCode = problemDetails.Status!.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}