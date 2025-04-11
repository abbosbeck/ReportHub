using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Invoices;

public class DeleteInvoiceCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteInvoiceCommandHandler(
    IInvoiceRepository repository,
    IMapper mapper
    )
    : IRequestHandler<DeleteInvoiceCommand, bool>
{
    public async Task<bool> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteAsync(request.Id);

        if (!result)
        {
            throw new NotFoundException("Invoice not found");
        }

        return result;
    }
}
