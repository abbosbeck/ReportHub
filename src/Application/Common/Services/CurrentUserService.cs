using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Services;

public class CurrentUserService(
    IClientRoleAssignmentRepository clientRoleAssignmentRepository,
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                          throw new ForbiddenException("User Context is unavailable");

    public List<string> SystemRoles => httpContextAccessor.HttpContext?.User.GetRoles() ?? new List<string> { };

    public async Task<List<JwtClientRole>> ClientRolesAsync()
    {
        var clientIdsandRoles = await clientRoleAssignmentRepository.GetRolesByUserIdAsync(UserId);

        return clientIdsandRoles;
    }
}