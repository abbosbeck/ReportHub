using System.Text.Json.Serialization;

namespace Application.Reports.ExportReportsToFile.Request;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsReportTableType
{
    Invoices,
    Items,
    Plans,
}
