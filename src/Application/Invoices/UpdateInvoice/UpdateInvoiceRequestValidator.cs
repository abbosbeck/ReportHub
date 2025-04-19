namespace Application.Invoices.UpdateInvoice;

public class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequest>
{
    public UpdateInvoiceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DueDate);
        RuleFor(x => x.PaymentStatus).IsInEnum();
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Name).NotEmpty();
            item.RuleFor(i => i.Price).GreaterThan(0);
            item.RuleFor(i => i.CurrencyCode).NotEmpty();
        });
    }
}
