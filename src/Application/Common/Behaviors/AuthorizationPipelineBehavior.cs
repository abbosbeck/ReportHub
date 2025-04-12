using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;

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

        var systemRoles = currentUserService.SystemRoles;
        var clientRoles = currentUserService.ClientRoles();

        if (requiresSystemRoles != null && requiresSystemRoles.Intersect(systemRoles).Any() && requiresClientRoles is null or[])
        {
            return await next();
        }

        // 👇 This logic should be more complex.
        // We have to check if the user has at least one of the required client roles for each client with clientId.
        if (requiresClientRoles != null && requiresClientRoles.Intersect(systemRoles).Any() && requiresSystemRoles is null or[])
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }
}