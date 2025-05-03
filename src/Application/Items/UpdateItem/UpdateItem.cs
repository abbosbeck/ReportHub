using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;

namespace Application.Items.UpdateItem;

public class UpdateItemCommand(Guid clientId, UpdateItemRequest item) : IRequest<ItemDto>, IClientRequest
{
    public UpdateItemRequest Item { get; set; } = item;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdateItemCommandHandler(
    IMapper mapper,
    IItemRepository itemRepository,
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    IValidator<UpdateItemRequest> validator,
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<UpdateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

        var isCurrencyCodeValid = await currencyExchangeService.CheckCurrencyCodeAsync(request.Item.CurrencyCode);
        if (!isCurrencyCodeValid)
        {
            throw new NotFoundException($"This CurrencyCode is not found with this id: {request.Item.CurrencyCode} " +
                                        $"https://www.exchangerate-api.com/docs/supported-currencies");
        }

        var item = await itemRepository.GetByIdAsync(request.Item.Id)
            ?? throw new NotFoundException($"Item is not found with this id: {request.Item.Id}");

        var invoice = await invoiceRepository.GetByIdAsync(item.InvoiceId)
            ?? throw new NotFoundException($"Invoice is not found with this id: {item.InvoiceId}");

        _ = await customerRepository.GetByIdAsync(invoice.CustomerId)
            ?? throw new NotFoundException($"Customer is not found with this id: {invoice.CustomerId}");

        var exchangedOldPrice = await currencyExchangeService
            .ExchangeCurrencyAsync(item.CurrencyCode, invoice.CurrencyCode, item.Price, invoice.IssueDate);

        var exchangedNewPrice = await currencyExchangeService
            .ExchangeCurrencyAsync(request.Item.CurrencyCode, invoice.CurrencyCode, request.Item.Price, invoice.IssueDate);

        invoice.Amount -= exchangedOldPrice;
        invoice.Amount += exchangedNewPrice;

        mapper.Map(request.Item, item);
        item.ClientId = request.ClientId;

        await invoiceRepository.UpdateAsync(invoice);
        await itemRepository.UpdateAsync(item);

        return mapper.Map<ItemDto>(item);
    }
}
