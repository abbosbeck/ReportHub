using Domain.Entities;

namespace Application.Invoices.GetInvoiceById;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(i => i.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<Item, ItemDto>();
    }
}
