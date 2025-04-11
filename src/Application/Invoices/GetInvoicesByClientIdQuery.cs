using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Invoices;

public class GetInvoicesByClientIdQuery : IRequest<List<InvoiceDto>>
{
    public Guid clientId { get; set; }
}

public class GetInvoicesByClientIdQueryHandler(
   IInvoiceRepository repository,
   IMapper mapper
   )
   : IRequestHandler<GetInvoicesByClientIdQuery, List<InvoiceDto>>
{
    public async Task<List<InvoiceDto>> Handle(GetInvoicesByClientIdQuery request, CancellationToken cancellationToken)
    {
        var invoices = await repository.GetByClientIdAsync(request.clientId);
        if (invoices == null || !invoices.Any())
        {
            throw new NotFoundException("No invoices found for the specified client.");
        }

        var activeInvoices = invoices.Where(i => !i.IsDeleted).ToList();
        return mapper.Map<List<InvoiceDto>>(activeInvoices);
    }
}
