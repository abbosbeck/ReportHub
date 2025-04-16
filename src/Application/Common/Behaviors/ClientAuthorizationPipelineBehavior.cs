using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Common.Behaviors;

public class ClientAuthorizationPipelineBehavior<TRequest, TResponse>(
    IClientRoleAssignmentRepository clientRoleAssignmentRepository,
    ICurrentUserService currentUserService,
    IRequestHandler<TRequest, TResponse> handler)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IClientRequest
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requiresClientRoles = handler.GetType().GetCustomAttribute<RequiresClientRoleAttribute>()?.ClientRoles;

        if (requiresClientRoles is null or[])
        {
            return await next();
        }

        var clientRoles =
            await clientRoleAssignmentRepository.GetRolesByUserIdAndClientIdAsync(currentUserService.UserId, request.ClientId);

        if (requiresClientRoles.Intersect(clientRoles).Any())
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }
}