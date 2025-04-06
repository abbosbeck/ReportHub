using Application.Common.Attributes;

namespace Application.Users.RefreshToken
{
    [AllowedFor]
    public class RefreshTokenRequest : IRequest<AccessTokenDto>
    {
        public string RefreshToken { get; set; }
    }
}
