using Application.Common.Constants;

namespace Application.Users.AssignRoleToUser;

public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserCommandValidator()
    {
        RuleFor(t => t.UserId)
            .NotEmpty();

        RuleFor(t => t.RoleName)
            .NotEmpty()
            .MaximumLength(64)
            .Must(role => role.Equals(SystemRoles.SuperAdmin) || role.Equals(SystemRoles.Regular));
    }
}
