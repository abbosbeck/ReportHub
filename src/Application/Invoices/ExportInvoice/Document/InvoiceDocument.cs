using System.Globalization;
using System.Reflection.Metadata;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Invoices.ExportInvoice.Document;

public class InvoiceDocument(Invoice invoice) : IDocument
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        var logo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logo.jpg");

        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item()
                    .Text($"Invoice #{invoice.InvoiceNumber:000000}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{invoice.IssueDate:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{invoice.DueDate:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Payment status: ").SemiBold();
                    text.Span($"{invoice.PaymentStatus:G}");
                });
            });

            row.ConstantItem(120).Height(60).Image(logo);
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new ClientComponent("Client", invoice.Client));
                row.ConstantItem(50);
                row.RelativeItem().Component(new CustomerComponent("Customer", invoice.Customer));
            });

            column.Item().Element(ComposeTable);
            column.Item().AlignRight().Text($"Grand total: {GetAmountWithSymbol(invoice.Amount, invoice.CurrencyCode)}").FontSize(14);
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(2);
                columns.RelativeColumn(4);
                columns.RelativeColumn(2);
                columns.RelativeColumn(1);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Name");
                header.Cell().Element(CellStyle).PaddingLeft(30).Text("Description");
                header.Cell().Element(CellStyle).AlignCenter().Text("Price");
                header.Cell().Element(CellStyle).AlignCenter().Text("Currency");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            foreach (var item in invoice.Items)
            {
                table.Cell().Element(CellStyle).Text($"{invoice.Items.IndexOf(item) + 1}");
                table.Cell().Element(CellStyle).Text(item.Name);
                table.Cell().Element(CellStyle).Text(item.Description);
                table.Cell().Element(CellStyle).Text($"{item.Price}");
                table.Cell().Element(CellStyle).AlignCenter().Text($"{item.CurrencyCode}");

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }

    private static string GetAmountWithSymbol(decimal amount, string currencyCode)
    {
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            var region = new RegionInfo(culture.Name);

            if (region.ISOCurrencySymbol.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
            {
                // var symbol = region.CurrencySymbol;

                var money = amount.ToString("C", culture);

                return money;
            }
        }

        return $"{amount:N2} {currencyCode}";
    }
}