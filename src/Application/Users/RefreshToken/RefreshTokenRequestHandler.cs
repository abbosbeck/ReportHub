using Application.Common.Interfaces;

namespace Application.Users.RefreshToken
{
    public class RefreshTokenRequestHandler(
        IUserRepository repository,
        IJwtTokenGenerator jwtTokenGenerator)
        : IRequestHandler<RefreshTokenRequest, AccessTokenDto>
    {
        public async Task<AccessTokenDto> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var user = await repository.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshToken != request.RefreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
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
}
