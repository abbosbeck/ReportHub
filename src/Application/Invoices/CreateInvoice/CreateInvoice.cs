using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Invoices;

public class CreateInvoiceCommand : IRequest<InvoiceDto>, IClientRequest
{
    public CreateInvoiceCommand(Guid clientId, CreateInvoiceRequest request)
    {
        ClientId = clientId;
        Invoice = request;
    }

    public CreateInvoiceRequest Invoice { get; set; }

    public Guid ClientId { get; set; }
}

public class AddInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepository,
    ICustomerRepository customerRepository,
    ICurrencyExchange currencyExchange,
    IMapper mapper,
    IValidator<CreateInvoiceRequest> validator
    ) : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        //await validator.ValidateAndThrowAsync(request.Invoice, cancellationToken);

        var existInvoice = await invoiceRepository.GetByInvoiceNumberAsync(request.Invoice.InvoiceNumber);
        if(existInvoice != null)
        {
            throw new ConflictException($"There is already an invoice with this invoice number: {request.Invoice.InvoiceNumber}");
        }

        var invoice = mapper.Map<Invoice>(request.Invoice);

        var customer = await customerRepository.GetAsync(c => c.Id == invoice.CustomerId)
            ?? throw new NotFoundException($"Customer is not found with this id: {invoice.CustomerId}");

        foreach (var itemDto in request.Invoice.Items)
        {
            var item = mapper.Map<Item>(itemDto);
            var exchangedPrice = await currencyExchange
                .ExchangeCurrencyAsync(item.CurrencyCode, customer.CountryCode, item.Price, invoice.IssueDate);

            invoice.Items.Add(item);
            invoice.Amount += exchangedPrice;
        }

        invoice.CurrencyCode = customer.CountryCode;
        await invoiceRepository.AddAsync(invoice);

        await itemRepository.AddBulkAsync(invoice.Items);

        return mapper.Map<InvoiceDto>(invoice);
    }
}