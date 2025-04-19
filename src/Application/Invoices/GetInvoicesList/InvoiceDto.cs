using Domain.Enums;

namespace Application.Invoices.GetInvoicesList;

public class InvoiceDto
{
    public Guid Id { get; init; }

    public string InvoiceNumber { get; init; }

    public DateTime IssueDate { get; init; }

    public DateTime DueDate { get; init; }

    public decimal Amount { get; init; }

    public string CurrencyCode { get; init; }

    public Guid ClientId { get; init; }

    public Guid CustomerId { get; init; }

    public InvoicePaymentStatus PaymentStatus { get; init; }

    public IEnumerable<ItemDto> Items { get; init; }
}