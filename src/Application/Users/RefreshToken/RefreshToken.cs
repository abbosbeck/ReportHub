using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Users.RefreshToken;

[AllowedFor]
public sealed class RefreshTokenQuery : IRequest<AccessTokenDto>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenQueryHandler(
    IUserRepository repository,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RefreshTokenQuery, AccessTokenDto>
{
    public async Task<AccessTokenDto> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByRefreshTokenAsync(request.RefreshToken);
        if (user == null || user.RefreshToken != request.RefreshToken
                         || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        var accessToken = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await repository.UpdateUserAsync(user);

        return new AccessTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}
