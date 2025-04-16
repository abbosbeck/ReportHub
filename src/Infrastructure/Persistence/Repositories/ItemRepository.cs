using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;
internal class ItemRepository(AppDbContext context) : IItemRepository
{
    public async Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<Guid> itemIds)
    {
        return await context.Set<Item>().Where(i => itemIds.Contains(i.Id)).ToListAsync();
    }
}
