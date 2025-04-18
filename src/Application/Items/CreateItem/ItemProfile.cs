using Domain.Entities;

namespace Application.Items.CreateItem;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<CreateItemCommand, Item>();
        CreateMap<Item, ItemDto>();
    }
}
