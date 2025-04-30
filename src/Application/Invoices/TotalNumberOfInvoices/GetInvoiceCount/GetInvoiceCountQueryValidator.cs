namespace Application.Invoices.TotalNumberOfInvoices.GetInvoiceCount;

public class GetInvoiceCountQueryValidator : AbstractValidator<GetInvoiceCountQuery>
{
    public GetInvoiceCountQueryValidator()
    {
        RuleFor(i => i.StartDate).NotEmpty();
        RuleFor(i => i.EndDate).NotEmpty().GreaterThan(i => i.StartDate);
    }
}
