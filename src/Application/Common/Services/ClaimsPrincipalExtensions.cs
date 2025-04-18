using System.Security.Claims;
using Application.Common.Exceptions;

namespace Application.Common.Services;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : Guid.Empty;
    }

    public static List<string> GetRoles(this ClaimsPrincipal principal)
    {
        var roleClaims = principal?.Claims.Where(claim => claim.Type == ClaimTypes.Role)
            ?? new List<Claim> { }.AsReadOnly();

        var roles = roleClaims.Select(roleClaim => roleClaim.Value).ToList();

        return roles;
    }
}