namespace Application.Plans.CreatePlan;

public class CreatePlanRequest
{
    public string Title { get; set; }

    public List<PlanItemDto> PlanItemDtos { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid CustomerId { get; set; }
}
