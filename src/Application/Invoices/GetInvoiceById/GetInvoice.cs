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
    IInvoiceRepository repository,
    IMapper mapper)
    : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

        return mapper.Map<InvoiceDto>(invoice);
    }
}
