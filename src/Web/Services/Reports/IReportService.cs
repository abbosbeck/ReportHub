using Web.Models.Reports;

namespace Web.Services.Reports;

public interface IReportService
{
    Task<byte[]> DownloadReport(
        Guid clientId, DateTime startDate, DateTime endDate, ReportFileType fileType, ReportTableType tableType);

    Task<ReportScheduleOptions> GetAsync(Guid clientId);

    Task<ReportScheduleOptions> ScheduleAsync(Guid clientId, ReportScheduleOptions interval);
    
    Task<ReportScheduleOptions> ReScheduleAsync(Guid clientId, ReportScheduleOptions interval);

    Task<bool> StopAsync(Guid clientId);
}
