namespace Application.Clients.AddClientMember;

public class AddClientMemberValidator : AbstractValidator<AddClientMemberCommand>
{
    public AddClientMemberValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Please provide valid name!");

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200).
            WithMessage("Please provide valid email!");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(200)
            .WithMessage("Please provide valid password!");
    }
}
