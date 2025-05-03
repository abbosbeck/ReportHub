using System.Text.Json.Serialization;

namespace Application.ExportReports.ExportReportsToFile;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsReportTableType
{
    Invoices,
    Items,
    Plans,
}
