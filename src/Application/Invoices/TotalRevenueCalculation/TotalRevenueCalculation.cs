using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.TotalRevenueCalculation;

public class TotalRevenueCalculationQuery(Guid clientId, TotalRevenueCalculationRequest request)
    : IClientRequest, IRequest<TotalRevenueCalculationDto>
{
    public TotalRevenueCalculationRequest TotalRevenueCalucationRequest { get; set; } = request;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class TotalRevenueCalculationQueryHandler(
    IClientRepository clientRepository,
    IInvoiceRepository invoiceRepository,
    ICountryService countryService,
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<TotalRevenueCalculationQuery, TotalRevenueCalculationDto>
{
    public async Task<TotalRevenueCalculationDto> Handle(TotalRevenueCalculationQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var clientCurrencyCode = await countryService.GetCurrencyCodeByCountryCodeAsync(client.CountryCode);

        var invoices = await invoiceRepository.GetAll()
            .Where(
                invoice => invoice.IssueDate > request.TotalRevenueCalucationRequest.StartDate &&
                invoice.IssueDate < request.TotalRevenueCalucationRequest.EndDate)
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

        return new TotalRevenueCalculationDto
        {
            StartDate = request.TotalRevenueCalucationRequest.StartDate,
            EndDate = request.TotalRevenueCalucationRequest.EndDate,
            TotalRevenue = totalRevenue,
        };
    }
}
