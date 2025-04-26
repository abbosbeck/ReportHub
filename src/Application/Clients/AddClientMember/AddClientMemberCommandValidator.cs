using Application.Common.ExtensionMethods;

namespace Application.Clients.AddClientMember;

public class AddClientMemberCommandValidator : AbstractValidator<AddClientMemberCommand>
{
    public AddClientMemberCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty()
            .BeAValidGuid();

        RuleFor(c => c.UserId)
            .NotEmpty()
            .BeAValidGuid();
    }
}
