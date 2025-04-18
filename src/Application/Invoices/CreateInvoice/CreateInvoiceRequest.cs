using Domain.Enums;

namespace Application.Invoices;

public class CreateInvoiceRequest
{
    public string InvoiceNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public Guid CustomerId { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public List<ItemRequestDto> Items { get; set; }
}