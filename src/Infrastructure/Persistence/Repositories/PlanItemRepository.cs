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

    public async Task<List<PlanItem>> GetPlanItemsByPlanIdAsync(Guid id)
    {
        return await context.Set<PlanItem>()
            .Where(pi => pi.PlanId == id)
            .ToListAsync();
    }
}
