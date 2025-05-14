using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Reports.ExportReportsToFile.Request;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.ExportReportsToFile;

public class ExportReportsToFileQuery(
    Guid clinetId,
    DateTime startDate,
    DateTime endDate,
    ExportReportsFileType fileType,
    ExportReportsReportTableType? reportType)
    : IRequest<ExportReportsToFileDto>, IClientRequest
{
    public DateTime StartDate { get; set; } = startDate;

    public DateTime EndDate { get; set; } = endDate;

    public ExportReportsFileType ExportReportsFileType { get; set; } = fileType;

    public ExportReportsReportTableType? ReportType { get; set; } = reportType;

    public Guid ClientId { get; set; } = clinetId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class ExportReportsToFileQueryHandler(
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepository,
    IPlanRepository planRepository,
    IClientRepository clientRepository,
    ICurrencyExchangeService currencyExchangeService,
    ICountryService countryService,
    IReportGeneratorAsFileService fileGenerator,
    IMapper mapper)
    : IRequestHandler<ExportReportsToFileQuery, ExportReportsToFileDto>, IExportReportsToFileQueryHandler
{
    public async Task<ExportReportsToFileDto> Handle(ExportReportsToFileQuery request, CancellationToken cancellationToken)
    {
        List<Invoice> invoices = new List<Invoice>();
        List<Item> items = new List<Item>();
        List<PlanDto> planDtos = new List<PlanDto>();

        if (request.ExportReportsFileType.Equals(ExportReportsFileType.Excel))
        {
            invoices = await invoiceRepository.GetAll()
                .IgnoreQueryFilters()
                .Where(invoice =>
                    invoice.ClientId == request.ClientId
                    && !invoice.IsDeleted &&
                    invoice.IssueDate > request.StartDate
                    && invoice.IssueDate < request.EndDate)
                .ToListAsync(cancellationToken);

            items = await itemRepository.GetAll()
                .IgnoreQueryFilters()
                .Where(i => i.ClientId == request.ClientId)
                .ToListAsync(cancellationToken);

            planDtos = await GetPlansAsync(request, cancellationToken);
        }
        else
        {
            if (request.ReportType.Equals(ExportReportsReportTableType.Invoices))
            {
                invoices = await invoiceRepository.GetAll().ToListAsync(cancellationToken);
            }
            else if (request.ReportType.Equals(ExportReportsReportTableType.Items))
            {
                items = await itemRepository.GetAll().ToListAsync(cancellationToken);
            }
            else if (request.ReportType.Equals(ExportReportsReportTableType.Plans))
            {
                planDtos = await GetPlansAsync(request, cancellationToken);
            }
        }

        var result = fileGenerator.GenerateExcelFile(invoices, items, planDtos, request);

        return result;
    }

    public async Task<List<PlanDto>> GetPlansAsync(ExportReportsToFileQuery request, CancellationToken cancellationToken)
    {
        var plans = await planRepository.GetAll()
            .IgnoreQueryFilters()
            .Where(plan =>
                    plan.ClientId == request.ClientId
                    && !plan.IsDeleted
                    && plan.StartDate > request.StartDate
                    && plan.EndDate < request.EndDate)
            .ToListAsync(cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.ClientId);
        var clientCurrency = await countryService.GetCurrencyCodeByCountryCodeAsync(client.CountryCode);

        List<PlanDto> planDtos = new List<PlanDto>();
        var exchangedValueCache = new Dictionary<(string, string, DateTime), decimal>();
        foreach (var plan in plans)
        {
            var itemsGroup = plan.PlanItems
                .GroupBy(pl => pl.Item.CurrencyCode)
                .Select(group => new
                {
                    CurrencyCode = group.Key,
                    Amount = group.Sum(planItem => planItem.Item.Price * planItem.Quantity),
                }).ToList();

            var planDto = mapper.Map<PlanDto>(plan);
            var totalPrice = (await Task.WhenAll(itemsGroup.Select(async item =>
            {
                var key = (item.CurrencyCode, clientCurrency, plan.StartDate);
                if (!exchangedValueCache.TryGetValue(key, out var exchangedValue))
                {
                    exchangedValue = await currencyExchangeService
                        .ExchangeCurrencyAsync(item.CurrencyCode, clientCurrency, 1, plan.StartDate);
                    exchangedValueCache[key] = exchangedValue;
                }

                return exchangedValue * item.Amount;
            }))).Sum();

            planDto.TotalPrice = totalPrice;
            planDto.CurrencyCode = clientCurrency;
            planDtos.Add(planDto);
        }

        return planDtos;
    }
}
