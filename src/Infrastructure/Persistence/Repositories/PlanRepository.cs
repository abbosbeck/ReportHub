﻿using Application.Common.Interfaces.Repositories;
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

    public IQueryable<Plan> GetAll()
    {
        return context.Set<Plan>()
            .Include(p => p.PlanItems)
            .ThenInclude(pi => pi.Item);
    }

    public async Task<Plan> GetByIdAsync(Guid id)
    {
        return await context.Set<Plan>()
            .Include(p => p.PlanItems)
            .ThenInclude(pi => pi.Item)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> RemoveAsync(Plan plan)
    {
        context.Set<Plan>().Remove(plan);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Plan> UpdateAsync(Plan plan)
    {
        context.Set<Plan>().Update(plan);
        await context.SaveChangesAsync();

        return plan;
    }
}
