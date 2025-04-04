using Application.Users.LoginUser;

namespace Application.Users.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommandRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(u => u.Department).NotEmpty().WithMessage("Department is required");
            RuleFor(u => u.PhoneNumber).NotEmpty().MatchPhoneNumberRule("Please provide valid phone number");
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
