using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;
public interface IPlanRepository
{
    Task<Plan> AddAsync(Plan plan);

    Task<Plan> GetByIdAsync(Guid id);
}
