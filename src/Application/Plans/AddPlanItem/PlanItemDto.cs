namespace Application.Plans.AddPlanItem;

public class PlanItemDto
{
    public Guid ItemId { get; init; }

    public Guid PlanId { get; init; }

    public int Quantity { get; init; }
}