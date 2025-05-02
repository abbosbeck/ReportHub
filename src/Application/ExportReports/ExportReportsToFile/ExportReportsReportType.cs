using System.Text.Json.Serialization;

namespace Application.ExportReports.ExportReportsToFile;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportReportsReportType
{
    /// <summary>
    /// Default value
    /// </summary>
    None,

    /// <summary>
    /// Inovice
    /// </summary>
    Invoice,

    /// <summary>
    /// Item
    /// </summary>
    Item,

    /// <summary>
    /// Plan
    /// </summary>
    Plan,
}
