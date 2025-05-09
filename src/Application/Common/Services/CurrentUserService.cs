using Application.Common.Interfaces.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Common.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty;

    public List<string> SystemRoles() 
    {
        var identity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            return identity.FindAll("SystemRoles")
                .Select(x => x.Value)
                .ToList();
        }
        return new List<string>();
    } 
}