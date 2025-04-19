using Domain.Entities;

namespace Application.Invoices.CreateInvoice;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(i => i.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<Item, ItemDto>();
        CreateMap<CreateInvoiceRequest, Invoice>()
            .ForMember(i => i.Items, opt => opt.Ignore());

        CreateMap<ItemRequestDto, Item>();
    }
}