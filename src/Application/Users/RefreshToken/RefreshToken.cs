using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Users.RefreshToken;

[AllowedFor]
public sealed class RefreshTokenCommand : IRequest<AccessTokenDto>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenCommandHandler(
    IUserRepository repository,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RefreshTokenCommand, AccessTokenDto>
{
    public async Task<AccessTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
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