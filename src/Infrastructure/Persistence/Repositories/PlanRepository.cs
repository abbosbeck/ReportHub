using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;
public class PlanRepository(AppDbContext context) : IPlanRepository
{
    public async Task<Plan> AddAsync(Plan plan)
    {
        await context.AddAsync(plan);
        await context.SaveChangesAsync();

        return plan;
    }

    public async Task<Plan> GetByIdAsync(Guid id)
    {
        return await context.Plan.FindAsync(id);
    }
}
