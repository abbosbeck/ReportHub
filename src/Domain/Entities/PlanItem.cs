namespace Domain.Entities;
public class PlanItem
{
    public Guid ItemId { get; set; }

    public Guid PlanId { get; set; }

    public int Quantity { get; set; }
}
