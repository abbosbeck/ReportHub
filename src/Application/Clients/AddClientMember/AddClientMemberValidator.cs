namespace Application.Clients.AddClientMember;

public class AddClientMemberValidator : AbstractValidator<AddClientMemberCommand>
{
    public AddClientMemberValidator()
    {
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
