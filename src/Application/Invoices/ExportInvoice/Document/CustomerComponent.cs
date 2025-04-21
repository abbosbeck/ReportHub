using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Invoices.ExportInvoice.Document;

public class CustomerComponent(string title, Customer customer) : IComponent
{
    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().BorderBottom(1).PaddingBottom(5).Text(title).SemiBold();

            column.Item().Text(customer.Name);
            column.Item().Text(customer.Email);
            column.Item().Text(customer.CountryCode.ToUpper());
        });
    }
}