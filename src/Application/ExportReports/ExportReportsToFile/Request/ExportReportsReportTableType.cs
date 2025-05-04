using System.Text.Json.Serialization;

namespace Application.ExportReports.ExportReportsToFile.Request;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsReportTableType
{
    Invoices,
    Items,
    Plans,
}
