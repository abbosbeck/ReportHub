using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class PlanItemRepository(AppDbContext context) : IPlanItemRepository
{
    public async Task AddBulkAsync(List<PlanItem> planItems)
    {
        await context.PlanItems.AddRangeAsync(planItems);
        await context.SaveChangesAsync();
    }

    public async Task<PlanItem> AddAsync(PlanItem planItem)
    {
        await context.AddAsync(planItem);
        await context.SaveChangesAsync();

        return planItem;
    }

    public async Task<bool> DeleteAsync(PlanItem planItem)
    {
        context.Remove(planItem);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<PlanItem> GetByPlanAndItemIdAsync(Guid planId, Guid itemId)
    {
        return await context.PlanItems.FirstOrDefaultAsync(planItem =>
            planItem.PlanId == planId && planItem.ItemId == itemId);
    }

    public async Task<List<PlanItem>> GetPlanItemsByPlanIdAsync(Guid id)
    {
        return await context.Set<PlanItem>()
            .Where(pi => pi.PlanId == id)
            .ToListAsync();
    }

    public IQueryable<PlanItem> GetAll()
    {
        return context.PlanItems;
    }
}
