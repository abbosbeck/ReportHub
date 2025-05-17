using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Web.Authentication;
using Web.Models.Users;

namespace Web.Services.Users;

public class UserProviderService(IHttpContextAccessor httpContextAccessor) : IUserProviderService
{
    private readonly Dictionary<string, string> _store = new();
    private readonly object _lock = new();

    private string? GetUserId()
    {
        return UserIdCookieHelper.GetOrCreateUserId(httpContextAccessor.HttpContext!);
    }

    public void SetToken(string token)
    {
        lock (_lock)
        {
            _store[GetUserId()] = token;
        }
    }
    public string GetToken()
    {
        lock (_lock)
        {
            return _store.TryGetValue(GetUserId(), out var token) ? token : null;
        }
    }

    public string GetUserEmail()
    {
        var token = GetToken();
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
    }

    public void RemoveToken()
    {
        lock (_lock)
        {
            _store.Remove(GetUserId());
        }
    }

    public UserRoles GetRoles()
    {
        var token = GetToken();
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var systemRoles = jwt.Claims
            .Where(x => x.Type == "SystemRoles")
            .Select(x => x.Value)
            .ToList();
        
        var clientRoles = jwt.Claims
            .Where(x => x.Type == "ClientRoles")
            .Select(x => x.Value)
            .ToList();

        return new UserRoles()
        {
            SystemRoles = systemRoles,
            ClientRoles = clientRoles,
        };
    }
}
