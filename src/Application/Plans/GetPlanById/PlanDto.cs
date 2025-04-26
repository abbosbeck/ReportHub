using Domain.Entities;

namespace Application.Plans.GetPlanById;

public class PlanDto
{
    public Guid Id { get; init; }

    public string Title { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public decimal TotalPrice { get; init; }

    public Guid ClientId { get; init; }

    public IEnumerable<PlanItemDto> Items { get; init; }
}