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
            var title = exception.GetType().Name;
            var detail = exception.Message;
            var instance = context.Request.Path;

            var problemDetails = exception switch
            {
                ValidationException validationException =>
                    new ProblemDetails()
                    {
                        Title = title,
                        Detail = detail,
                        Type = "https://httpstatuses.com/400",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = instance,
                        Extensions =
                        {
                            ["errors"] = validationException.Errors,
                        },
                    },
                BadRequestException =>
                    new ProblemDetails
                    {
                        Title = title,
                        Detail = detail,
                        Type = "https://httpstatuses.com/400",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = instance,
                    },
                UnauthorizedException =>
                    new ProblemDetails
                    {
                        Title = title,
                        Detail = detail,
                        Type = "https://httpstatuses.com/401",
                        Status = StatusCodes.Status401Unauthorized,
                        Instance = instance,
                    },
                ForbiddenException =>
                    new ProblemDetails
                    {
                        Title = title,
                        Detail = detail,
                        Type = "https://httpstatuses.com/403",
                        Status = StatusCodes.Status403Forbidden,
                        Instance = instance,
                    },
                NotFoundException =>
                    new ProblemDetails
                    {
                        Title = title,
                        Detail = detail,
                        Type = "https://httpstatuses.com/404",
                        Status = StatusCodes.Status404NotFound,
                        Instance = instance,
                    },
                ConflictException =>
                    new ProblemDetails
                    {
                        Title = title,
                        Detail = detail,
                        Type = "https://httpsstatus.com/409",
                        Status = StatusCodes.Status409Conflict,
                        Instance = instance,
                    },
                _ => new ProblemDetails
                {
                    Title = title,
                    Detail = detail,
                    Type = "https://httpstatuses.com/500",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = instance,
                }
            };

            context.Response.StatusCode = problemDetails.Status!.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}