using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Invoices.GetInvoiceById;

public class GetInvoiceQuery : IRequest<InvoiceDto>, IClientRequest
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }
}

public class GetInvoiceQueryHandler(
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepository,
    IMapper mapper)
    : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

        var items = await itemRepository.GetByInvoiceIdAsync(invoice.Id)
            ?? throw new NotFoundException($"There is no item this invoice id: {request.Id}");

        invoice.Items = items;

        return mapper.Map<InvoiceDto>(invoice);
    }
}
