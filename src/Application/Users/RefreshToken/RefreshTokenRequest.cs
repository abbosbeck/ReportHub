namespace Application.Users.RefreshToken
{
    public class RefreshTokenRequest : IRequest<AccessTokenDto>
    {
        public string RefreshToken { get; set; }
    }
}
