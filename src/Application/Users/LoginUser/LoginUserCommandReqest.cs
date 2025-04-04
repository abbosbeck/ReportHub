namespace Application.Users.LoginUser
{
    public class LoginUserCommandReqest : IRequest<LoginUserDto>
    {
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
