
using Web.Models.Reports;

namespace Web.Services.Reports;

public class ReportService(IHttpClientFactory httpClientFactory) : IReportService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    private readonly Dictionary<ReportScheduleOptions, string> _intervals =
        new()
        {
            {
                ReportScheduleOptions.Minutely, "0 0/1 * * * ?"
            },
            {
                ReportScheduleOptions.Daily, "0 0/5 * * * ?"
            },
            {
                ReportScheduleOptions.Weekly, "0 0/5 * * * ?"
            },
            {
                ReportScheduleOptions.Monthly, "0 0/5 * * * ?"
            },
        };

    public async Task<byte[]> DownloadReport(
        Guid clientId, DateTime startDate, DateTime endDate, ReportFileType fileType, ReportTableType tableType)
    {
        var invoiceResponse = await _httpClient
            .GetAsync($"clients/{clientId}/reports/download?" +
            $"startDate={startDate}&endDate={endDate}&fileType={fileType}&reportTableType={tableType}");
        
        if (!invoiceResponse.IsSuccessStatusCode)
        {
            return [];
        }

        return await invoiceResponse.Content.ReadAsByteArrayAsync();
    }

    public async Task<ReportScheduleOptions> GetAsync(Guid clientId)
    {
        var response = await _httpClient.GetAsync($"clients/{clientId}/reports/me");

        if (!response.IsSuccessStatusCode)
        {
            return ReportScheduleOptions.Disabled;
        }

        var reportSchedule = await response.Content.ReadFromJsonAsync<ReportScheduleResponse>();

        return _intervals.First(interval => interval.Value == reportSchedule.CronExpression).Key;
    }

    public async Task<ReportScheduleOptions> ScheduleAsync(Guid clientId, ReportScheduleOptions interval)
    {
        var reportSchedule = new ReportScheduleRequest()
        {
            CronExpression = _intervals[interval]
        };

        var response = await _httpClient.PostAsJsonAsync($"clients/{clientId}/reports/schedule", reportSchedule);
        
        if (!response.IsSuccessStatusCode)
        {
            return ReportScheduleOptions.Disabled;
        }

        return interval;
    }

    public async Task<ReportScheduleOptions> ReScheduleAsync(Guid clientId, ReportScheduleOptions interval)
    {
        var reportSchedule = new ReportScheduleRequest()
        {
            CronExpression = _intervals[interval]
        };

        var response = await _httpClient.PutAsJsonAsync($"clients/{clientId}/reports/re-schedule", reportSchedule);
        
        if (!response.IsSuccessStatusCode)
        {
            return ReportScheduleOptions.Disabled;
        }

        return interval;
    }

    public async Task<bool> StopAsync(Guid clientId)
    {
        var response = await _httpClient.DeleteAsync($"clients/{clientId}/reports/stop");

        return response.IsSuccessStatusCode;
    }
}
