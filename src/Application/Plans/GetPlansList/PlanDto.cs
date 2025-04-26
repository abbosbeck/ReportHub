namespace Application.Plans.GetPlansList;

public class PlanDto
{
    public Guid Id { get; init; }

    public string Title { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public Guid ClientId { get; init; }
}