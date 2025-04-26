using Domain.Common;

namespace Domain.Entities;

public class PlanItem : BaseAuditableEntity, ISoftDeletable
{
    public Guid ItemId { get; set; }

    public Guid PlanId { get; init; }

    public int Quantity { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
