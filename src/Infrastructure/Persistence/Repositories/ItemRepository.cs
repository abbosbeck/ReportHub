using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ItemRepository(AppDbContext context) : IItemRepository
{
    public async Task<Item> GetByIdAsync(Guid id)
    {
        return await context.Items.FindAsync(id);
    }

    public async Task<Item> AddAsync(Item item)
    {
        await context.Items.AddAsync(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async Task<Item> UpdateAsync(Item item)
    {
        context.Update(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async Task<bool> DeleteAsync(Item item)
    {
        context.Remove(item);

        return await context.SaveChangesAsync() > 0;
    }

}
