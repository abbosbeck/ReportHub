using Domain.Common;

namespace Domain.Entities;

public class Plan : BaseAuditableEntity, ISoftDeletable
{
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public IList<PlanItem> PlanItems { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
