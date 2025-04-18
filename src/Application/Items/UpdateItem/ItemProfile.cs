using Domain.Entities;

namespace Application.Items.UpdateItem;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<UpdateItemRequest, Item>();
        CreateMap<Item, ItemDto>();
    }
}
