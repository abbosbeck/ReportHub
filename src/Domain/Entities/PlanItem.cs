using Domain.Common;

namespace Domain.Entities;

public class PlanItem : BaseAuditableEntity, ISoftDeletable
{
    public Guid ItemId { get; init; }

    public Item Item { get; init; }

    public Guid PlanId { get; init; }

    public Plan Plan { get; init; }

    public int Quantity { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
