using Domain.Common;

namespace Domain.Entities;

public class PlanItem : IAuditableEntity, ISoftDeletable
{
    public Guid ItemId { get; init; }

    public Item Item { get; init; }

    public Guid PlanId { get; init; }

    public Plan Plan { get; init; }

    public int Quantity { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string LastModifiedBy { get; set; }
}
