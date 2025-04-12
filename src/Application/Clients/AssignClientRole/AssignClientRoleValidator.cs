namespace Application.Clients.AssignClientRole;

public class AssignClientRoleValidator : AbstractValidator<AssignClientRoleCommand>
{
    public AssignClientRoleValidator()
    {
        RuleFor(c => c.RoleName)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(c => c.ClientId)
            .NotEmpty()
            .Must(BeAValidGuid);

        RuleFor(c => c.UserId)
            .NotEmpty()
            .Must(BeAValidGuid);
    }

    private static bool BeAValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
