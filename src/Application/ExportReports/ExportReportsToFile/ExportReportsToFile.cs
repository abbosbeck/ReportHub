using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Application.ExportReports.ExportReportsToFile.FileGenerators;
using Microsoft.EntityFrameworkCore;

namespace Application.ExportReports.ExportReportsToFile;

public class ExportReportsToFileQuery(Guid clinetId, ExportReportsFileType fileType) : IRequest<ExportReportsToFileDto>, IClientRequest
{
    public ExportReportsFileType ExportReportsFileType { get; set; } = fileType;

    public Guid ClientId { get; set; } = clinetId;
}

public class ExportReportsToFileQueryHandler(
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepository,
    IPlanRepository planRepository,
    IPlanItemRepository planItemRepository,
    IClientRepository clientRepository,
    ICurrencyExchangeService currencyExchangeService,
    ICountryService countryService,
    IMapper mapper)
    : IRequestHandler<ExportReportsToFileQuery, ExportReportsToFileDto>
{
    public async Task<ExportReportsToFileDto> Handle(ExportReportsToFileQuery request, CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetAll().ToListAsync(cancellationToken);
        var items = await itemRepository.GetAll().ToListAsync(cancellationToken);
        var plans = await planRepository.GetAll().ToListAsync(cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.ClientId);
        var clientCurrency = await countryService.GetCurrencyCodeByCountryCodeAsync(client.CountryCode);

        List<PlanDto> planDtos = new List<PlanDto>();
        foreach (var plan in plans)
        {
            var planDto = mapper.Map<PlanDto>(plan);
            var totalPrice = plan.PlanItems.Sum(planItem =>
            {
                var price = currencyExchangeService
                    .ExchangeCurrencyAsync(planItem.Item.CurrencyCode, clientCurrency, planItem.Item.Price, plan.StartDate)
                    .Result;

                return planItem.Quantity * price;
            });
            planDto.TotalPrice = currencyExchangeService.GetAmountWithSymbol(totalPrice, clientCurrency);
            planDto.CurrencyCode = clientCurrency;
            planDtos.Add(planDto);
        }

        new ExcelFileGenerator(currencyExchangeService)
            .GenerateExcelFile(invoices, items, planDtos);

        return new ExportReportsToFileDto();
    }
}
