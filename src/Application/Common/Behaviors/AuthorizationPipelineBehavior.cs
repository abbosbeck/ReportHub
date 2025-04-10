using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Common.Behaviors;

public class AuthorizationPipelineBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IRequestHandler<TRequest, TResponse> handler)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requiresSystemRoles = handler.GetType().GetCustomAttribute<RequiresSystemRoleAttribute>()?.SystemRoles;
        var requiresClientRoles = handler.GetType().GetCustomAttribute<RequiresClientRoleAttribute>()?.ClientRoles;

        if (requiresSystemRoles is null or[] && requiresClientRoles is null or[])
        {
            return await next();
        }

        var roles = currentUserService.Roles;

        if (requiresSystemRoles != null && requiresSystemRoles.Intersect(roles).Any() && requiresClientRoles is null or[])
        {
            return await next();
        }

        if (requiresClientRoles != null && requiresClientRoles.Intersect(roles).Any() && requiresSystemRoles is null or[])
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }
}