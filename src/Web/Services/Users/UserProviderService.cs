using System.IdentityModel.Tokens.Jwt;
using Web.Models.Users;

namespace Web.Services.Users;

public class UserProviderService : IUserProviderService
{
    private string _accessToken;

    public void SetToken(string token) => _accessToken = token;
    public string GetToken() => _accessToken;

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

    public void RemoveToken() => _accessToken = null;

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
