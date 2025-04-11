namespace Application.Invoices;

public class AddInvoiceCommandValidator : AbstractValidator<AddInvoiceCommand>
{
    public AddInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceNumber).NotEmpty();
        RuleFor(x => x.IssueDate).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty().GreaterThan(x => x.IssueDate);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.PaymentStatus).IsInEnum();
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Name).NotEmpty();
            item.RuleFor(i => i.Price).GreaterThan(0);
            item.RuleFor(i => i.Currency).NotEmpty();
        });
    }
}
