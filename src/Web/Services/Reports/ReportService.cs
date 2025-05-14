
using Web.Models.Reports;

namespace Web.Services.Reports;

public class ReportService(IHttpClientFactory httpClientFactory) : IReportService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

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
}
