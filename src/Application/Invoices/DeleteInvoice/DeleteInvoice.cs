using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Invoices;

public class DeleteInvoiceCommand : IRequest<bool>, IClientRequest
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner)]
public class DeleteInvoiceCommandHandler(IInvoiceRepository repository)
    : IRequestHandler<DeleteInvoiceCommand, bool>
{
    public async Task<bool> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

        var result = await repository.DeleteAsync(invoice);

        return result;
    }
}