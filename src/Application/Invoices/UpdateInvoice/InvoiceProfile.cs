using Domain.Entities;

namespace Application.Invoices.UpdateInvoice;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<Item, ItemDto>();
        CreateMap<UpdateInvoiceCommand, Invoice>();
        CreateMap<ItemRequestDto, Item>();
    }
}