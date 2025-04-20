using System.Reflection.Metadata.Ecma335;

namespace Application.Plans.CreatePlan;

public class PlanItemDto
{
    public Guid ItemId { get; set; }

    public int Quantity { get; set; }
}
