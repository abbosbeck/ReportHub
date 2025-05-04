using Application.Reports.ExportReportsToFile;
using Application.Reports.ExportReportsToFile.Request;
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
