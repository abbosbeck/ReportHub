using Domain.Entities;

namespace Application.Invoices.CreateInvoice;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<Item, ItemDto>();
        CreateMap<CreateInvoiceRequest, Invoice>()
            .ForMember(x => x.Items, opt => opt.Ignore());

        CreateMap<ItemRequestDto, Item>();
    }
}