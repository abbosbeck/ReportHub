namespace Application.Plans.CreatePlan;

public class CreatePlanRequest
{
    public string Title { get; set; }

    public ICollection<Guid> ItemIds { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
