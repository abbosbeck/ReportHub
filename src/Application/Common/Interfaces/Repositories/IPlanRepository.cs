using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;
public interface IPlanRepository
{
    Task<Plan> AddAsync(Plan plan);

    IQueryable<Plan> GetAll();

    Task<Plan> GetByIdAsync(Guid id);

    Task<bool> RemoveAsync(Plan plan);

    Task<Plan> UpdateAsync(Plan plan);
}
