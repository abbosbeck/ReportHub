﻿using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
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
    ICountryService countryService,
    ICurrencyExchangeService currencyExchange,
    IMapper mapper,
    IValidator<CreateInvoiceRequest> validator)
    : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Invoice, cancellationToken);

        var invoice = mapper.Map<Invoice>(request.Invoice);

        var customer = await customerRepository.GetByIdAsync(invoice.CustomerId)
            ?? throw new NotFoundException($"Customer is not found with this id: {invoice.CustomerId}");

        var customerCurrency = await countryService.GetCurrencyCodeByCountryCodeAsync(customer.CountryCode);

        foreach (var itemDto in request.Invoice.Items)
        {
            var item = mapper.Map<Item>(itemDto);

            var exchangedPrice = await currencyExchange
                .ExchangeCurrencyAsync(item.CurrencyCode, customerCurrency, item.Price, invoice.IssueDate);
            invoice.Amount += exchangedPrice;
        }

        invoice.CurrencyCode = customerCurrency;
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