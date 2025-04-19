using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class PlanItemRepository(AppDbContext context) : IPlanItemRepository
{
    public async Task<List<Guid>> GetItemIdsByPlanIdAsync(Guid id)
    {
        return await context.Set<PlanItem>()
            .Where(pi => pi.PlanId == id)
            .Select(pi => pi.ItemId)
            .ToListAsync();
    }
}
