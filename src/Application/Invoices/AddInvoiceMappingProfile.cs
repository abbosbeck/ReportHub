using Application.ItemsFolder;
using Domain.Entities;

namespace Application.Invoices;

public class AddInvoiceMappingProfile : Profile
{
    public AddInvoiceMappingProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<Item, ItemDto>();
        CreateMap<AddInvoiceCommand, Invoice>();
        CreateMap<ItemInputDto, Item>();
    }
}
