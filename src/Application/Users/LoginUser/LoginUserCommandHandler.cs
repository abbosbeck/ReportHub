using Application.Common.Interfaces;
using Domain.Entity;
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
            var user = await repository.GetUserByPhoneNumberAsync(request.PhoneNumber);
            if (user == null)
            {
                throw new SecurityTokenException("Invalid phone number or password!");
            }

            var passwordVerificationResult = passwordHasher
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new SecurityTokenException("Invalid phone number or password");
            }

            var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
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
