using Domain.Enums;

namespace Application.Invoices.CreateInvoice;

public class CreateInvoiceRequest
{
    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public Guid CustomerId { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public List<ItemRequestDto> Items { get; set; }
}