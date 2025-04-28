namespace Application.Plans.AddPlanItem;

public abstract class AddPlanItemRequest
{
    public Guid ItemId { get; init; }

    public int Quantity { get; init; }
}