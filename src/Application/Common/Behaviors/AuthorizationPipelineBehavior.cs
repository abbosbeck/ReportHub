using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Common.Behaviors;

public class AuthorizationPipelineBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository,
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

        if (requiresSystemRoles != null && requiresSystemRoles.Intersect(systemRoles).Any())
        {
            return await next();
        }

        var clientRoles = await GetClientRoles(request);

        if (requiresClientRoles != null && requiresClientRoles.Intersect(clientRoles).Any())
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }

    private async Task<List<string>> GetClientRoles(TRequest request)
    {
        if (request is IClientRequest clientRequest)
        {
            return await clientRoleAssignmentRepository
                .GetRolesByUserIdAndClientIdAsync(currentUserService.UserId, clientRequest.ClientId);
        }

        return [];
    }
}
