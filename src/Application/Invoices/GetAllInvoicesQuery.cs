using Application.Common.Interfaces;

namespace Application.Invoices;

public class GetAllInvoicesQuery : IRequest<List<InvoiceDto>>
{
}

public class GetAllInvoicesQueryHandler(
IInvoiceRepository repository,
IMapper mapper)
: IRequestHandler<GetAllInvoicesQuery, List<InvoiceDto>>
{
    public async Task<List<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await repository.GetAllAsync();
        var activeInvoices = invoices.Where(i => !i.IsDeleted).ToList();

        return mapper.Map<List<InvoiceDto>>(activeInvoices);
    }
}
