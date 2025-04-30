using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.GetOverdueInvoicePaymentsAnalysis;

public class GetOverdueInvoicePaymentsAnalysisQuery(Guid clientId)
    : IRequest<OverdueInvoicePaymentsAnalysisDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.Owner, ClientRoles.Operator)]
public class GetOverdueInvoicePaymentsAnalysisQueryHandler(
    ICountryService countryService,
    IDateTimeService dateTimeService,
    IClientRepository clientRepository,
    IInvoiceRepository invoiceRepository,
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<GetOverdueInvoicePaymentsAnalysisQuery, OverdueInvoicePaymentsAnalysisDto>
{
    public async Task<OverdueInvoicePaymentsAnalysisDto> Handle(
        GetOverdueInvoicePaymentsAnalysisQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId);
        var clientCurrencyCode = await countryService.GetCurrencyCodeByCountryCodeAsync(client.CountryCode);
        var today = dateTimeService.UtcNow.Date;

        var invoices = await invoiceRepository.GetAll()
            .Where(invoice =>
                invoice.PaymentStatus != InvoicePaymentStatus.Paid &&
                invoice.DueDate < today)
            .Select(invoice => new
            {
                invoice.CurrencyCode,
                invoice.Amount,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        if (invoices.Count == 0)
        {
            throw new NotFoundException("No Overdue Invoice Payments are found");
        }

        var result = await Task.WhenAll(
            invoices.Select(invoice => currencyExchangeService.ExchangeCurrencyAsync(
                invoice.CurrencyCode, clientCurrencyCode, invoice.Amount, today)));

        return new OverdueInvoicePaymentsAnalysisDto
        {
            CurrencyCode = clientCurrencyCode,
            TotalAmount = result.Sum(),
            NumberOfInvoices = result.Length,
        };
    }
}