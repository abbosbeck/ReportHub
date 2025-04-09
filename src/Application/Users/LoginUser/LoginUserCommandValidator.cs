namespace Application.Users.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Email).EmailAddress().WithMessage("Please provide valid email");
    }
}