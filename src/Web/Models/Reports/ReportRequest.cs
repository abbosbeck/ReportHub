namespace Web.Models.Reports;

public class ReportRequest
{
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today;
    public ReportFileType FileType { get; set; } = ReportFileType.Excel;
    public ReportTableType TableType { get; set; } = ReportTableType.Invoices;
}
