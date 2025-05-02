using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IPlanItemRepository
{
    Task<List<PlanItem>> GetPlanItemsByPlanIdAsync(Guid id);

    Task AddBulkAsync(List<PlanItem> planItems);

    Task<PlanItem> AddAsync(PlanItem planItem);

    Task<bool> DeleteAsync(PlanItem planItem);

    Task<PlanItem> GetByPlanAndItemIdAsync(Guid planId, Guid itemId);

    IQueryable<PlanItem> GetAll();
}
