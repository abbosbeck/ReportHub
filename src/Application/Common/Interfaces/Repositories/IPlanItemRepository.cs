using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IPlanItemRepository
{
    Task<List<PlanItem>> GetPlanItemsByPlanIdAsync(Guid id);

    Task AddBulkAsync(List<PlanItem> planItems);
}
