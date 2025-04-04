namespace Application.Users.RegisterUser
{
    public class RegisterUserCommandRequest : IRequest<RegisterUserDto>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
