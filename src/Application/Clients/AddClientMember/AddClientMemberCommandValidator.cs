namespace Application.Clients.AddClientMember;

public class AddClientMemberCommandValidator : AbstractValidator<AddClientMemberCommand>
{
    public AddClientMemberCommandValidator()
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
