using Domain.Entities;

namespace Application.Items.GetItemsList;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>();
    }
}
