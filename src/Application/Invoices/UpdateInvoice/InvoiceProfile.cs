using Application.Invoices.CreateInvoice;
using Domain.Entities;

namespace Application.Invoices.UpdateInvoice;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Item, ItemDto>();

        CreateMap<UpdateInvoiceRequest, Invoice>()
            .ForMember(i => i.Items, opt => opt.Ignore());

        CreateMap<ItemRequestDto, Item>();
    }
}