using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Domain.Entities;

namespace Application.Users.LoginUser;

public sealed class LoginUserCommand : IRequest<LoginDto>
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IDateTimeService dateTimeService,
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<LoginUserCommand> validator)
        : IRequestHandler<LoginUserCommand, LoginDto>
{
    public async Task<LoginDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var user = await userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedException("Invalid email or password");

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedException("Email is not confirmed");
        }

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        var accessToken = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = dateTimeService.UtcNow.AddDays(7);

        _ = await userRepository.UpdateAsync(user);

        return new LoginDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}