using Web.Models.Items;

namespace Web.Services.Items;

public interface IItemService
{
    Task<List<ItemResponse>> GetItemListAsync(Guid clientId);
}
