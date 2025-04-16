namespace Application.Common.Configurations;

public class JwtOptions
{
    public string Key { get; init; }

    public string Issuer { get; init; }

    public string Audience { get; init; }

    public int AccessTokenExpiryMinutes { get; init; }

    public int RefreshTokenExpiryDays { get; init; }
}
