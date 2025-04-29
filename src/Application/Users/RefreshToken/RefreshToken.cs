using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;

namespace Application.Users.RefreshToken;

public sealed class RefreshTokenCommand : IRequest<AccessTokenDto>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenCommandHandler(
    IUserRepository repository,
    IDateTimeService dateTimeService,
    IJwtTokenGenerator jwtTokenGenerator,
    IValidator<RefreshTokenCommand> validator)
    : IRequestHandler<RefreshTokenCommand, AccessTokenDto>
{
    public async Task<AccessTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var user = await repository.GetByRefreshTokenAsync(request.RefreshToken);
        if (user == null || user.RefreshToken != request.RefreshToken
                         || user.RefreshTokenExpiryTime <= dateTimeService.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        var accessToken = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = dateTimeService.UtcNow.AddDays(7);

        await repository.UpdateAsync(user);

        return new AccessTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}