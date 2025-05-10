using Web.Models.Items;

namespace Web.Services.Items;

public class ItemService : IItemService
{
    public Task<List<ItemResponse>> GetItemListAsync(Guid clientId)
    {
        throw new NotImplementedException();
    }
}
