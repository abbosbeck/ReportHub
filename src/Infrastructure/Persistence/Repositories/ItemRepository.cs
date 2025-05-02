using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ItemRepository(AppDbContext context) : IItemRepository
{
    public async Task<Item> AddAsync(Item item)
    {
        await context.AddAsync(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async Task AddBulkAsync(ICollection<Item> items)
    {
        context.Items.AddRange(items);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Item item)
    {
        context.Remove(item);

        return await context.SaveChangesAsync() > 0;
    }

    public IQueryable<Item> GetAll()
    {
        return context.Items.Include(i => i.Invoice).AsQueryable();
    }

    public async Task<Item> GetByIdAsync(Guid itemId)
    {
        return await context.Items.FindAsync(itemId);
    }

    public async Task<List<Item>> GetByInvoiceIdAsync(Guid id)
    {
        return await context.Items
            .Where(i => i.InvoiceId == id)
            .ToListAsync();
    }

    public async Task<Item> UpdateAsync(Item item)
    {
        context.Update(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<Guid> itemIds)
    {
        return await context.Set<Item>().Where(i => itemIds.Contains(i.Id)).ToListAsync();
    }
}
