using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Invoices.CreateInvoice;

public class CreateInvoiceCommand(Guid clientId, CreateInvoiceRequest request)
    : IRequest<InvoiceDto>, IClientRequest
{
    public CreateInvoiceRequest Invoice { get; set; } = request;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepository,
    ICustomerRepository customerRepository,
    ICurrencyExchangeService currencyExchange,
    IMapper mapper,
    IValidator<CreateInvoiceRequest> validator)
    : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Invoice, cancellationToken);

        var existInvoice = await invoiceRepository.GetByInvoiceNumberAsync(request.Invoice.InvoiceNumber);
        if (existInvoice != null)
        {
            throw new ConflictException($"There is already an invoice with this invoice number: {request.Invoice.InvoiceNumber}");
        }

        var invoice = mapper.Map<Invoice>(request.Invoice);

        var customer = await customerRepository.GetByIdAsync(invoice.CustomerId)
            ?? throw new NotFoundException($"Customer is not found with this id: {invoice.CustomerId}");

        foreach (var itemDto in request.Invoice.Items)
        {
            var item = mapper.Map<Item>(itemDto);
            var exchangedPrice = await currencyExchange
                .ExchangeCurrencyAsync(item.CurrencyCode, customer.CountryCode, item.Price, invoice.IssueDate);
            invoice.Amount += exchangedPrice;
        }

        invoice.CurrencyCode = customer.CountryCode;
        invoice.ClientId = request.ClientId;
        await invoiceRepository.AddAsync(invoice);

        var itemsList = new List<Item>();
        foreach (var itemDto in request.Invoice.Items)
        {
            var item = mapper.Map<Item>(itemDto);
            item.InvoiceId = invoice.Id;
            item.ClientId = request.ClientId;
            itemsList.Add(item);
        }

        await itemRepository.AddBulkAsync(itemsList);

        return mapper.Map<InvoiceDto>(invoice);
    }
}