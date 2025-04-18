namespace Application.Items.CreateItem;

public class CreateItemCommandValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3);

        RuleFor(c => c.InvoiceId)
            .NotEmpty()
            .Must(BeAValidGuid);
    }

    private static bool BeAValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
