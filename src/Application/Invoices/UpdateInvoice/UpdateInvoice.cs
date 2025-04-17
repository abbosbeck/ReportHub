using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Invoices.UpdateInvoice;

public class UpdateInvoiceCommand : IRequest<InvoiceDto>
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public Guid ClientId { get; set; }

    public Guid CustomerId { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public List<ItemInputDto> Items { get; set; }
}

public class UpdateInvoiceCommandHandler(
    IInvoiceRepository repository,
    IValidator<UpdateInvoiceCommand> validator,
    IMapper mapper)
    : IRequestHandler<UpdateInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        _ = await repository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        _ = await repository.GetByIdAsync(request.CustomerId)
            ?? throw new NotFoundException($"Customer is not found wirt this id: {request.CustomerId}");

        var invoice = await repository.GetByIdAsync(request.Id);

        mapper.Map(request, invoice);

        await repository.UpdateAsync(invoice);

        return mapper.Map<InvoiceDto>(invoice);
    }
}
