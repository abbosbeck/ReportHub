namespace Application.Clients.DeleteClient;

public class DeleteClientValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientValidator()
    {
            RuleFor(c => c.ClientId)
                .NotEmpty()
                .Must(BeAValidGuid);
    }

    private static bool BeAValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
