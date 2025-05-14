using Web.Models.Reports;

namespace Web.Services.Reports;

public interface IReportService
{
    Task<byte[]> DownloadReport(
        Guid clientId, DateTime startDate, DateTime endDate, ReportFileType fileType, ReportTableType tableType);
}
