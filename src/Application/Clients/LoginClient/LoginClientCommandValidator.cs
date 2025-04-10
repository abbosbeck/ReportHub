namespace Application.Clients.LoginClient;

public class LoginClientCommandValidator : AbstractValidator<LoginClientCommand>
{
    public LoginClientCommandValidator()
    {
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Email).EmailAddress().WithMessage("Please provide valid email");
    }
}