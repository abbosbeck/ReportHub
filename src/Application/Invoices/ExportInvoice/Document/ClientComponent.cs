using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Invoices.ExportInvoice.Document;

public class ClientComponent(string title, Client client) : IComponent
{
    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().BorderBottom(1).PaddingBottom(5).Text(title).SemiBold();

            column.Item().Text(client.Name);
        });
    }
}