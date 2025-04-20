namespace Application.Plans.UpdatePlan;

public class UpdatePlanRequest
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}