using Domain.Entities;

namespace Application.Invoices.GetInvoiceById;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>();
    }
}
