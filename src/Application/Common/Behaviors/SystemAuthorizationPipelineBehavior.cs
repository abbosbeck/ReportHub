using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Common.Behaviors;

public class SystemAuthorizationPipelineBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IRequestHandler<TRequest, TResponse> handler)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requiresSystemRoles = handler.GetType().GetCustomAttribute<RequiresSystemRoleAttribute>()?.SystemRoles;

        if (requiresSystemRoles is null or[])
        {
            return await next();
        }

        var systemRoles = currentUserService.SystemRoles;

        if (requiresSystemRoles.Intersect(systemRoles).Any())
        {
            return await next();
        }

        throw new ForbiddenException("You are not allowed to perform this action");
    }
}