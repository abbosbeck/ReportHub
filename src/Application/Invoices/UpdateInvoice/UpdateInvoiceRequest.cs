using Application.Invoices.CreateInvoice;
using Domain.Enums;

namespace Application.Invoices.UpdateInvoice;

public class UpdateInvoiceRequest
{
    public Guid Id { get; set; }

    public DateTime DueDate { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public List<ItemRequestDto> Items { get; set; }
}