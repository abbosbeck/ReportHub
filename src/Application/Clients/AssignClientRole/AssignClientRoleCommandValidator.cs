using Application.Common.ExtensionMethods;

namespace Application.Clients.AssignClientRole;

public class AssignClientRoleCommandValidator : AbstractValidator<AssignClientRoleCommand>
{
    public AssignClientRoleCommandValidator()
    {
        RuleFor(c => c.RoleName)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(c => c.ClientId)
            .NotEmpty()
            .BeAValidGuid();

        RuleFor(c => c.UserId)
            .NotEmpty()
            .BeAValidGuid();
    }
}
