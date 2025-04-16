using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;
public interface IItemRepository
{
    Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<Guid> itemIds);
}
