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
        var clientRolesWithClientIds = await currentUserService.ClientRolesAsync();

        if (requiresSystemRoles != null && requiresSystemRoles.Intersect(systemRoles).Any() && requiresClientRoles is null or[])
        {
            return await next();
        }

        var clientId = ResolveClientId(request)
        ?? throw new ForbiddenException("ClientId is required for client-scoped actions.");

        List<string> clientRoles = clientRolesWithClientIds?
            .Where(x => x.ClientId == clientId)
            .Select(x => x.RoleName)
            .ToList() ?? [];

        if (requiresClientRoles != null && requiresClientRoles.Intersect(clientRoles).Any() && requiresSystemRoles is null or[])
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }

    private static Guid? ResolveClientId(TRequest request)
    {
       if (request is IClientRequest clientRequest)
       {
           return clientRequest.ClientId;
       }

       return null;
    }
}