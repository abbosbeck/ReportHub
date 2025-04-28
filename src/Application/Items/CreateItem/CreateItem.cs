using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.CreateItem;

public class CreateItemCommand(Guid clientId, CreateItemRequest item) : IRequest<ItemDto>, IClientRequest
{
    public CreateItemRequest Item { get; set; } = item;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class CreateItemCommandHandler(
    IMapper mapper,
    IItemRepository itemRepository,
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    IValidator<CreateItemRequest> validator,
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<CreateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

        var item = mapper.Map<Item>(request.Item);
        item.ClientId = request.ClientId;

        var isCurrencyCodeValid = await currencyExchangeService.CheckCurrencyCodeAsync(item.CurrencyCode);
        if (!isCurrencyCodeValid)
        {
            throw new NotFoundException($"This CurrencyCode is not found with this id: {item.CurrencyCode}\n" +
                                        $"https://www.exchangerate-api.com/docs/supported-currencies");
        }

        var invoice = await invoiceRepository.GetByIdAsync(item.InvoiceId)
            ?? throw new NotFoundException($"Invoice is not found with this id: {item.InvoiceId}");

        _ = await customerRepository.GetByIdAsync(invoice.CustomerId)
            ?? throw new NotFoundException($"Customer is not found with this id: {invoice.CustomerId}");

        var exchangedPrice = await currencyExchangeService
            .ExchangeCurrencyAsync(item.CurrencyCode, invoice.CurrencyCode, item.Price, invoice.IssueDate);

        invoice.Amount += exchangedPrice;

        await invoiceRepository.UpdateAsync(invoice);

        await itemRepository.AddAsync(item);

        return mapper.Map<ItemDto>(item);
    }
}
