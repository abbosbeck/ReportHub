namespace Application.Plans.GetPlansList;

public class PlanDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid ClientId { get; set; }
}