using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Users.LoginUser;

[AllowedFor]
public sealed class LoginUserCommand : IRequest<LoginUserDto>
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class LoginUserCommandHandler(
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<LoginUserCommand> validator)
        : IRequestHandler<LoginUserCommand, LoginUserDto>
{
    public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedException("Invalid email or password");

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedException("Email is not confirmed");
        }

        var passwordVerificationResult = await userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordVerificationResult)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        var accessToken = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return new LoginUserDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}
