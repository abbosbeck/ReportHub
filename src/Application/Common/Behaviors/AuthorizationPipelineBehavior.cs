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
        var requiredRoles = handler.GetType().GetCustomAttribute<AllowedForAttribute>()?.Roles;
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