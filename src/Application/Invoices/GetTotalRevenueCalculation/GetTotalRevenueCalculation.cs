﻿using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.GetTotalRevenueCalculation;

public class GetTotalRevenueCalculationQuery(Guid clientId, DateTime startDate, DateTime endDate)
    : IClientRequest, IRequest<GetTotalRevenueCalculationDto>
{
    public DateTime StartDate { get; set; } = startDate;

    public DateTime EndDate { get; set; } = endDate;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetTotalRevenueCalculationQueryHandler(
    IClientRepository clientRepository,
    IInvoiceRepository invoiceRepository,
    ICountryService countryService,
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<GetTotalRevenueCalculationQuery, GetTotalRevenueCalculationDto>
{
    public async Task<GetTotalRevenueCalculationDto> Handle(GetTotalRevenueCalculationQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var clientCurrencyCode = await countryService.GetCurrencyCodeByCountryCodeAsync(client.CountryCode);

        var invoices = await invoiceRepository.GetAll()
            .Where(
                invoice => invoice.IssueDate > request.StartDate &&
                invoice.IssueDate < request.EndDate)
            .Select(invoice => new
            {
                invoice.Amount,
                invoice.CurrencyCode,
                invoice.IssueDate,
            })
            .ToListAsync(cancellationToken);

        if (invoices.Count == 0)
        {
            throw new NotFoundException("No invoices issued in selected period");
        }

        var result = await Task.WhenAll(
            invoices.Select(invoice => currencyExchangeService.ExchangeCurrencyAsync(
                invoice.CurrencyCode, clientCurrencyCode, invoice.Amount, invoice.IssueDate)));

        var totalRevenue = currencyExchangeService.GetAmountWithSymbol(result.Sum(), clientCurrencyCode);

        return new GetTotalRevenueCalculationDto(
            totalRevenue, clientCurrencyCode, request.StartDate, request.EndDate);
    }
}
