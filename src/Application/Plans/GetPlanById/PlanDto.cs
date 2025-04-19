using Domain.Entities;

namespace Application.Plans.GetPlanById;

public class PlanDto
{
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public List<ItemDto> Items { get; set; }

    public decimal TotalPrice { get; set; }

    public Guid ClientId { get; set; }
}