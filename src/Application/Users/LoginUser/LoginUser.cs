using Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Application.Users.LoginUser
{
    public class LoginUserCommandReqest : IRequest<LoginUserDto>
    {
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public class LoginUserCommandHandler(
            IUserRepository repository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher)
            : IRequestHandler<LoginUserCommandReqest, LoginUserDto>
        {
            public async Task<LoginUserDto> Handle(LoginUserCommandReqest request, CancellationToken cancellationToken)
            {
                var user = await repository.GetUserByPhoneNumberAsync(request.PhoneNumber);
                if (user == null)
                {
                    throw new SecurityTokenException("User with specified number was not found!");
                }

                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user.PasswordHash, request.Password);

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
}
