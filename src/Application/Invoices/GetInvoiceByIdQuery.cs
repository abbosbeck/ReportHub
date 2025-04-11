using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Invoices;

public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
{
    public Guid Id { get; set; }
}

public class GetInvoiceByIdQueryHandler(
   IInvoiceRepository repository,
   IMapper mapper
   )
   : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.Id);
        if (invoice == null || invoice.IsDeleted)
        {
            throw new NotFoundException("Invoice not found.");
        }

        return mapper.Map<InvoiceDto>(invoice);
    }
}
