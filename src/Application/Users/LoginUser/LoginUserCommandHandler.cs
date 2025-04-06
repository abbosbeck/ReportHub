using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Users.LoginUser
{
    public class LoginUserCommandHandler(
            IUserRepository repository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher<User> passwordHasher)
            : IRequestHandler<LoginUserCommandReqest, LoginUserDto>
    {
        public async Task<LoginUserDto> Handle(LoginUserCommandReqest request, CancellationToken cancellationToken)
        {
            var user = await repository.GetUserByPhoneNumberAsync(request.PhoneNumber)
                ?? throw new UnauthorizedException("Invalid phone number or password");

            var passwordVerificationResult = passwordHasher
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedException("Invalid phone number or password");
            }

            var accessToken = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
            var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await repository.UpdateUserAsync(user);

            return new LoginUserDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
