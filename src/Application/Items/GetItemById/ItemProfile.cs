using Domain.Entities;

namespace Application.Items.GetItemById;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>();
    }
}
