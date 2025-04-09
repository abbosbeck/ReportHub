using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Item : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Price { get; set; }

    public ItemPaymentStatus PaymentStatus { get; set; }

    public string Currency { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; }

    public Guid InvoiceId { get; set; }

    public Invoice Invoice { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
