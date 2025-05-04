using System.Drawing;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Application.ExportReports.ExportReportsToFile;
using Aspose.Cells;
using Domain.Entities;

namespace Application.Common.Services;

public class ReportGeneratorAsFileService : IReportGeneratorAsFileService
{
    private readonly Style headerStyle = new SheetStyle().CreateHeaderStyle();
    private readonly Style cellBorderSyle = new SheetStyle().CreateBorderStyle();

    public ExportReportsToFileDto GenerateExcelFile(
        List<Invoice> invoices,
        List<Item> items,
        List<PlanDto> plans,
        ExportReportsFileType fileType,
        ExportReportsReportTableType? reportType)
    {
        Workbook mainWorkbook = new Workbook();
        WorksheetCollection sheets = mainWorkbook.Worksheets;

        MemoryStream ms = new MemoryStream();

        if (fileType == ExportReportsFileType.CSV)
        {
            Worksheet dataSheet = sheets[0];
            if (reportType == ExportReportsReportTableType.Invoices)
            {
                AddInvoiceSheet(dataSheet, invoices);
                dataSheet.Name = "Invoices";
            }
            else if (reportType == ExportReportsReportTableType.Items)
            {
                AddItemSheet(dataSheet, items);
                dataSheet.Name = "Items";
            }
            else if (reportType == ExportReportsReportTableType.Plans)
            {
                AddPlanSheet(dataSheet, plans);
                dataSheet.Name = "Plans";
            }
            else
            {
                throw new BadRequestException("Please choose a table: Invoices, Items, or Plans.");
            }

            mainWorkbook.Save(ms, SaveFormat.Csv);

            mainWorkbook.Worksheets[0].AutoFitColumns();

            return new ExportReportsToFileDto(
                ms.ToArray(),
                "text/csv",
                reportType.ToString());
        }
        else
        {
            AddInvoiceSheet(sheets.Add("Invoices"), invoices);
            AddItemSheet(sheets.Add("Items"), items);
            AddPlanSheet(sheets.Add("Plans"), plans);

            mainWorkbook.Save(ms, SaveFormat.Xlsx);

            return new ExportReportsToFileDto(
                    ms.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Reports");
        }
    }

    private void AddPlanSheet(Worksheet worksheet, List<PlanDto> plans)
    {
        var cells = worksheet.Cells;
        cells[0, 0].Value = "No";
        cells[0, 1].Value = "Title";
        cells[0, 2].Value = "Start Date";
        cells[0, 3].Value = "End Date";
        cells[0, 4].Value = "Total Price";
        cells[0, 5].Value = "Currency Code";

        for (int col = 0; col < 6; col++)
        {
            cells[0, col].SetStyle(headerStyle);
            worksheet.Cells.SetColumnWidth(col, 18);
        }

        int row = 1;
        int counter = 1;
        foreach (var plan in plans)
        {
            cells[row, 0].Value = counter++;
            cells[row, 1].Value = plan.Title;
            cells[row, 2].Value = string.Format("{0:MM/dd/yyyy}", plan.StartDate);
            cells[row, 3].Value = string.Format("{0:MM/dd/yyyy}", plan.EndDate);
            cells[row, 4].Value = plan.TotalPrice;
            cells[row, 5].Value = plan.CurrencyCode;

            for (int col = 0; col < 6; col++)
            {
                cells[row, col].SetStyle(cellBorderSyle);
            }

            row++;
        }

        worksheet.FreezePanes(1, 0, 1, 6);
        worksheet.AutoFitColumns();
    }

    private void AddItemSheet(Worksheet worksheet, List<Item> items)
    {
        var cells = worksheet.Cells;
        cells[0, 0].Value = "No";
        cells[0, 1].Value = "Name";
        cells[0, 2].Value = "Description";
        cells[0, 3].Value = "Price";
        cells[0, 4].Value = "Currency Code";
        cells[0, 5].Value = "Invoice Number";

        for (int col = 0; col < 6; col++)
        {
            cells[0, col].SetStyle(headerStyle);
            worksheet.Cells.SetColumnWidth(col, 18);
        }

        int row = 1;
        int counter = 1;
        foreach (var item in items)
        {
            cells[row, 0].Value = counter++;
            cells[row, 1].Value = item.Name;
            cells[row, 2].Value = item.Description;
            cells[row, 3].Value = string.Format("{0:#.00}", Convert.ToDecimal(item.Price) / 100);
            cells[row, 4].Value = item.CurrencyCode;
            cells[row, 5].Value = item.Invoice.InvoiceNumber.ToString("D6");

            for (int col = 0; col < 6; col++)
            {
                var cell = cells[row, col];
                cell.SetStyle(cellBorderSyle);
            }

            row++;
        }

        worksheet.FreezePanes(1, 0, 1, 6);
        worksheet.AutoFitColumns();
    }

    private void AddInvoiceSheet(Worksheet worksheet, List<Invoice> invoices)
    {
        var cells = worksheet.Cells;
        cells[0, 0].Value = "No";
        cells[0, 1].Value = "Invoice Number";
        cells[0, 2].Value = "Issue Date";
        cells[0, 3].Value = "Due Date";
        cells[0, 4].Value = "Amount";
        cells[0, 5].Value = "Currency Code";
        cells[0, 6].Value = "Payment Status";

        for (int col = 0; col < 7; col++)
        {
            cells[0, col].SetStyle(headerStyle);
            worksheet.Cells.SetColumnWidth(col, 18);
        }

        int row = 1;
        int counter = 1;
        foreach (var invoice in invoices)
        {
            cells[row, 0].Value = counter++;
            cells[row, 1].Value = invoice.InvoiceNumber.ToString("D6");
            cells[row, 2].Value = string.Format("{0:MM/dd/yyyy}", invoice.IssueDate);
            cells[row, 3].Value = string.Format("{0:MM/dd/yyyy}", invoice.DueDate);
            cells[row, 4].Value = string.Format("{0:#.00}", Convert.ToDecimal(invoice.Amount) / 100);
            cells[row, 5].Value = invoice.CurrencyCode;
            cells[row, 6].Value = invoice.PaymentStatus.ToString();

            for (int col = 0; col < 7; col++)
            {
                var cell = cells[row, col];
                cell.SetStyle(cellBorderSyle);
            }

            row++;
        }

        worksheet.FreezePanes(1, 0, 1, 7);
        worksheet.AutoFitColumns();
    }
}

public class SheetStyle
{
    private readonly CellsFactory cellsFactory = new CellsFactory();

    public Style CreateHeaderStyle()
    {
        Style style = cellsFactory.CreateStyle();
        style.HorizontalAlignment = TextAlignmentType.Center;
        style.VerticalAlignment = TextAlignmentType.Center;
        style.Font.IsBold = true;
        style.ForegroundColor = Color.MediumSlateBlue;
        style.Pattern = BackgroundType.Solid;
        style.Font.Color = Color.White;
        return style;
    }

    public Style CreateBorderStyle()
    {
        Style style = cellsFactory.CreateStyle();
        style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
        style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style.Pattern = BackgroundType.Solid;
        return style;
    }
}
