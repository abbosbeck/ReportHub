using Application.Common.ExtensionMethods;

namespace Application.Clients.DeleteClient;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
            RuleFor(c => c.ClientId)
                .NotEmpty()
                .BeAValidGuid();
    }
}
