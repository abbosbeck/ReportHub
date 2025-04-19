using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.GetInvoicesList;

public class GetInvoicesListQuery(Guid clientId) : IRequest<IEnumerable<InvoiceDto>>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

public class GetInvoicesListQueryHandler(IMapper mapper, IInvoiceRepository repository)
    : IRequestHandler<GetInvoicesListQuery, IEnumerable<InvoiceDto>>
{
    public async Task<IEnumerable<InvoiceDto>> Handle(GetInvoicesListQuery request, CancellationToken cancellationToken)
    {
        var invoices = await repository
            .GetAll()
            .ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }
}