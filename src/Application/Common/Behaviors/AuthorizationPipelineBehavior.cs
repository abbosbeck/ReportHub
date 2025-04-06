using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Common.Behaviors;

public class AuthorizationPipelineBehavior<TRequest, TResponse>(ICurrentUserService currentUserService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IBaseRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requiredRoles = request.GetType().GetCustomAttribute<AllowedForAttribute>()?.Roles;
        if (requiredRoles is null or[])
        {
            return await next();
        }

        var roles = currentUserService.Roles;
        if (requiredRoles.Union(roles).Count() < requiredRoles.Length + roles.Count)
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }
}