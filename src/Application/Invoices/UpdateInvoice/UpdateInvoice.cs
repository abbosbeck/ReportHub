using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Invoices.UpdateInvoice;

public class UpdateInvoiceCommand : IRequest<InvoiceDto>, IClientRequest
{
    public UpdateInvoiceCommand(Guid clientId, UpdateInvoiceRequest request)
    {
        ClientId = clientId;
        Invoice = request;
    }

    public UpdateInvoiceRequest Invoice { get; set; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    IItemRepository itemRepository,
    ICurrencyExchangeService currencyExchange,
    IValidator<UpdateInvoiceRequest> validator,
    IMapper mapper)
    : IRequestHandler<UpdateInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Invoice, cancellationToken);

        var invoice = await invoiceRepository.GetByIdAsync(request.Invoice.Id)
            ?? throw new NotFoundException($"Invoice is not found with this id: {request.Invoice.Id}");

        if (request.Invoice.DueDate <= invoice.IssueDate)
        {
            throw new BadRequestException("Due Date should be greater than Issue Date");
        }

        var customer = await customerRepository.GetByIdAsync(invoice.CustomerId)
            ?? throw new NotFoundException($"Customer is not found wirt this id: {invoice.CustomerId}");

        mapper.Map(request.Invoice, invoice);

        foreach (var itemDto in request.Invoice.Items)
        {
            var item = mapper.Map<Item>(itemDto);
            var exchangedPrice = await currencyExchange
                .ExchangeCurrencyAsync(item.CurrencyCode, customer.CountryCode, item.Price, invoice.IssueDate);
            invoice.Amount += exchangedPrice;
        }

        await invoiceRepository.UpdateAsync(invoice);

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
