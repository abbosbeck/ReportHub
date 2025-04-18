using Domain.Entities;

namespace Application.Items.UpdateItem;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<UpdateItemCommand, Item>();
        CreateMap<Item, ItemDto>();
    }
}
