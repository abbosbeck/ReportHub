using Domain.Entities;

namespace Application.Items.CreateItem;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<CreateItemRequest, Item>();
        CreateMap<Item, ItemDto>();
    }
}
