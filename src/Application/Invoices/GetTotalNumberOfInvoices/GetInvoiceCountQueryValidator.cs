namespace Application.Invoices.GetTotalNumberOfInvoices;

public class GetInvoiceCountQueryValidator : AbstractValidator<GetInvoiceCountQuery>
{
    public GetInvoiceCountQueryValidator()
    {
        RuleFor(query => query.StartDate).NotEmpty();
        RuleFor(query => query.EndDate).NotEmpty().GreaterThan(query => query.StartDate);
    }
}
