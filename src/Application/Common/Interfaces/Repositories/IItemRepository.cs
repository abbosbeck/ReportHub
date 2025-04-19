using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IItemRepository
{
    IQueryable<Item> GetAll();

    Task<Item> GetByIdAsync(Guid itemId);

    Task<Item> AddAsync(Item item);

    Task AddBulkAsync(ICollection<Item> items);

    Task<List<Item>> GetByInvoiceIdAsync(Guid id);

    Task<Item> UpdateAsync(Item item);

    Task<bool> DeleteAsync(Item item);
}
