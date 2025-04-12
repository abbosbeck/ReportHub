using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Common.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                          throw new ForbiddenException("User Context is unavailable");

    public List<string> Roles => httpContextAccessor.HttpContext?.User.GetRoles() ?? new List<string> { };

    public List<JwtClientRole> ClientRoles()
    {
        var clientRolesJson = httpContextAccessor.HttpContext?.User.FindFirst("ClientRoles")?.Value;
        var clientRoles = JsonConvert.DeserializeObject<List<JwtClientRole>>(clientRolesJson ?? string.Empty);

        return clientRoles;
    }
}