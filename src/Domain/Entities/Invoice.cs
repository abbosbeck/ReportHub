using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Invoice : BaseAuditableEntity, ISoftDeletable
{
    public string InvoiceNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; }

    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
