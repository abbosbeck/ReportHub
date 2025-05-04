using Application.ExportReports.ExportReportsToFile;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IReportGeneratorAsFileService
{
    ExportReportsToFileDto GenerateExcelFile(
        List<Invoice> invoices,
        List<Item> items,
        List<PlanDto> plans,
        ExportReportsFileType fileType,
        ExportReportsReportTableType? reportType);
}
