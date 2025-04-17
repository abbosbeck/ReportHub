using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ItemRepository(AppDbContext context) : IItemRepository
{
    public async Task<Item> AddAsync(Item item)
    {
        await context.Items.AddAsync(item);
        await context.SaveChangesAsync();

        return item;
    }
}
