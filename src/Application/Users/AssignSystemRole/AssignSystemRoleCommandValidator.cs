using Application.Common.Constants;

namespace Application.Users.AssignSystemRole;

public class AssignSystemRoleCommandValidator : AbstractValidator<AssignSystemRoleCommand>
{
    public AssignSystemRoleCommandValidator()
    {
        RuleFor(t => t.UserId)
            .NotEmpty();

        RuleFor(t => t.RoleName)
            .NotEmpty()
            .MaximumLength(64)
            .Must(role => role.Equals(SystemRoles.SuperAdmin) || role.Equals(SystemRoles.Regular));
    }
}
