namespace Application.Clients.UpdateClient;

public class ClientValidator : AbstractValidator<UpdateClientCommand>
{
    public ClientValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(c => c.ClientId)
            .NotEmpty()
            .Must(BeAValidGuid);
    }

    private static bool BeAValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
