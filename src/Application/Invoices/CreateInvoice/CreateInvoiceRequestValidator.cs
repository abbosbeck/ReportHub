namespace Application.Invoices.CreateInvoice;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.IssueDate).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty().GreaterThan(x => x.IssueDate);
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.PaymentStatus).IsInEnum();
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Name).NotEmpty();
            item.RuleFor(i => i.Price).GreaterThan(0);
            item.RuleFor(i => i.CurrencyCode).NotEmpty();
        });
    }
}