﻿using Application.Common.Interfaces.External.CurrencyExchange;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Invoices.ExportInvoice.Document;

public class InvoiceDocument(Invoice invoice, ICurrencyExchangeService currency) : IDocument
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
            var amount = currency.GetAmountWithSymbol(invoice.Amount, invoice.CurrencyCode);
            column.Item().AlignRight().Text($"Grand total: {amount}").FontSize(14);
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
                columns.RelativeColumn(5);
                columns.RelativeColumn(2);
                columns.RelativeColumn(1.2f);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Name");
                header.Cell().Element(CellStyle).PaddingLeft(30).Text("Description");
                header.Cell().Element(CellStyle).AlignLeft().Text("Price");
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
                table.Cell().Element(CellStyle).PaddingHorizontal(10).Text(item.Description);
                table.Cell().Element(CellStyle).Text($"{item.Price}");
                table.Cell().Element(CellStyle).AlignCenter().Text($"{item.CurrencyCode}");

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }
}