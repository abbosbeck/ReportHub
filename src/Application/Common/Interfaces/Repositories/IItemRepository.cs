using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IItemRepository
{
    Task<Item> AddAsync(Item item);
}
