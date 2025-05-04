using System.Text.Json.Serialization;

namespace Application.Reports.ExportReportsToFile.Request;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsFileType
{
    Excel,
    CSV,
}