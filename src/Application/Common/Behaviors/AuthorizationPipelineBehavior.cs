using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Common.Behaviors;

public class AuthorizationPipelineBehavior<TRequest, TResponse>(
    IClientIdProvider clientIdProvider,
    ICurrentUserService currentUserService,
    IRequestHandler<TRequest, TResponse> handler,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requiresSystemRoles = handler.GetType().GetCustomAttribute<RequiresSystemRoleAttribute>()?.SystemRoles;
        var requiresClientRoles = handler.GetType().GetCustomAttribute<RequiresClientRoleAttribute>()?.ClientRoles;

        var systemRoles = currentUserService.SystemRoles;
        var clientRoles = await GetClientRoles(request);

        if (requiresSystemRoles is null or[] && requiresClientRoles is null or[])
        {
            return await next();
        }

        if (requiresSystemRoles != null && requiresSystemRoles.Intersect(systemRoles).Any())
        {
            return await next();
        }

        if (requiresClientRoles != null && requiresClientRoles.Intersect(clientRoles).Any())
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }

    private async Task<List<string>> GetClientRoles(TRequest request)
    {
        if (request is not IClientRequest clientRequest)
        {
            return new List<string>();
        }

        clientIdProvider.ClientId = clientRequest.ClientId;

        return await clientRoleAssignmentRepository
            .GetRolesByUserIdAndClientIdAsync(currentUserService.UserId, clientRequest.ClientId);
    }
}
