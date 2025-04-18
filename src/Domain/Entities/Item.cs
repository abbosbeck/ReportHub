using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Item : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }

    public Guid ClientId { get; set; }

    public Client Client { get; init; }

    public Guid InvoiceId { get; init; }

    public Invoice Invoice { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
