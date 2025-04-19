namespace Application.Invoices.DeleteInvoice;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(i => i.Id)
            .NotEmpty()
            .Must(BeValidGuid);

        RuleFor(i => i.ClientId)
            .NotEmpty()
            .Must(BeValidGuid);
    }

    public static bool BeValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
