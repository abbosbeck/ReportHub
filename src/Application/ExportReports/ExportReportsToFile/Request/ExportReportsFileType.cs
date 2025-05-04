using System.Text.Json.Serialization;

namespace Application.ExportReports.ExportReportsToFile.Request;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsFileType
{
    Excel,
    CSV,
}