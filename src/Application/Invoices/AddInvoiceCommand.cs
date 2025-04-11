using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Invoices;

public class AddInvoiceCommand : IRequest<InvoiceDto>
{
    public string InvoiceNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public Guid ClientId { get; set; }

    public Guid CustomerId { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public List<ItemInputDto> Items { get; set; }
}

public class AddInvoiceCommandHandler(
    IInvoiceRepository repository,
    IMapper mapper,
    IValidator<AddInvoiceCommand> validator
    )
    : IRequestHandler<AddInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var existingInvoices = await repository.GetAllAsync();
        if (existingInvoices.Any(i => i.InvoiceNumber == request.InvoiceNumber))
        {
            throw new ConflictException("An invoice with this number already exists.");
        }

        var invoice = mapper.Map<Invoice>(request);
        invoice.Items = new List<Item>();

        if (request.Items != null)
        {
            foreach (var itemDto in request.Items)
            {
                var item = mapper.Map<Item>(itemDto);
                invoice.Items.Add(item);
            }
        }

        await repository.AddAsync(invoice);
        var newInvoice = await repository.GetByIdAsync(invoice.Id);

        if (newInvoice == null)
        {
            throw new NotFoundException("Failed to retrieve the newly created invoice.");
        }

        return mapper.Map<InvoiceDto>(newInvoice);
    }
}