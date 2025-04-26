using Application.Common.ExtensionMethods;

namespace Application.Clients.CreateClient;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(c => c.CountryCode)
            .NotEmpty();

        RuleFor(c => c.OwnerId)
            .NotEmpty()
            .BeAValidGuid();
    }
}
