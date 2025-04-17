using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Invoices.GetInvoice;

public class GetInvoiceCommand : IRequest<InvoiceDto>
{
    public Guid Id { get; set; }
}

public class GetInvoiceCommandHandler(
    IInvoiceRepository repository,
    IMapper mapper)
    : IRequestHandler<GetInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

        return mapper.Map<InvoiceDto>(invoice);
    }
}
