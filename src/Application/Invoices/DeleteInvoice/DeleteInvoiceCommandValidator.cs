namespace Application.Invoices.DeleteInvoice;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(I => I.Id)
            .NotEmpty()
            .Must(BeValidGuid);
    }

    public static bool BeValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
