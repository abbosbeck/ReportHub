using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Invoice : BaseAuditableEntity, ISoftDeletable
{
    public string InvoiceNumber { get; init; }

    public DateTime IssueDate { get; init; }

    public DateTime DueDate { get; init; }

    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; }

    public ICollection<Item> Items { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; init; }

    public Guid CustomerId { get; set; }

    public Customer Customer { get; init; }

    public InvoicePaymentStatus PaymentStatus { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
