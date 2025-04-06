using Application.Common.Attributes;
using Application.Common.Constants;

namespace Application.Users.LoginUser
{
    [AllowedFor]
    public class LoginUserCommandReqest : IRequest<LoginUserDto>
    {
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
