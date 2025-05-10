using Web.Models.Items;

namespace Web.Services.Items;

public interface IItemService
{
    Task<List<ItemResponse>> GetItemListAsync(Guid clientId);

    Task<bool> CreateAsync(CreateItemRequest item, Guid clientId);

    Task<bool> DeleteAsync(Guid id, Guid clientId);
}
