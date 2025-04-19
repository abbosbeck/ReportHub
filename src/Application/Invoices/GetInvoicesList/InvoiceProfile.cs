using Domain.Entities;

namespace Application.Invoices.GetInvoicesList;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<Item, ItemDto>();
    }
}
