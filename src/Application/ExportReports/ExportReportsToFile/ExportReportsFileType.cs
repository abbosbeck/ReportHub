using System.Text.Json.Serialization;

namespace Application.ExportReports.ExportReportsToFile;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsFileType
{
    /// <summary>
    /// If excel file should be generated
    /// </summary>
    Excel,

    /// <summary>
    /// If CVS file should be generated
    /// </summary>
    CSV,
}