using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IPlanItemRepository
{
    Task<List<Guid>> GetItemIdsByPlanIdAsync(Guid id);
}
