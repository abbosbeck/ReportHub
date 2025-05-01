using Application.Common.Interfaces.Authorization;
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
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<ExportReportsToFileQuery, ExportReportsToFileDto>
{
    public async Task<ExportReportsToFileDto> Handle(ExportReportsToFileQuery request, CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetAll().ToListAsync(cancellationToken);

        new ExcelFileGenerator(currencyExchangeService)
            .GenerateInvoice(invoices);

        return new ExportReportsToFileDto();
    }
}
