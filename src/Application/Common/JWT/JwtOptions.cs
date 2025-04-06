namespace Application.Common.JWT;

public class JwtOptions
{
    public string Key { get; init; }

    public string Issuer { get; init; }

    public string Audience { get; init; }

    public int AccessTokenExpiryMinutes { get; init; }

    public int RefreshTokenExpiryDays { get; init; }
}
