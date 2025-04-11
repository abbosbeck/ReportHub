using Application.ItemsFolder;
using Domain.Enums;

namespace Application.Invoices
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public Guid ClientId { get; set; }

        public Guid CustomerId { get; set; }

        public InvoicePaymentStatus PaymentStatus { get; set; }

        public List<ItemDto> Items { get; set; }

    }

}
