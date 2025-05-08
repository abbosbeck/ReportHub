using System.IdentityModel.Tokens.Jwt;

namespace Web.Services;

public class TokenProviderService : ITokenProviderService
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
}
