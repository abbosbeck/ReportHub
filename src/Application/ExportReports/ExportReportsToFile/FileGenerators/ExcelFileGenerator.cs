using System.Data;
using System.Drawing;
using Application.Common.Interfaces.External.CurrencyExchange;
using Aspose.Cells;
using Domain.Entities;

namespace Application.ExportReports.ExportReportsToFile.FileGenerators;

public class ExcelFileGenerator(ICurrencyExchangeService currencyExchangeService)
{
    public void GenerateInvoice(List<Invoice> invoices)
    {
        Workbook workbook = new Workbook();
        DataTable invoiceTable = new DataTable("Invoice");

        invoiceTable.Columns.Add("No", typeof(long));
        invoiceTable.Columns.Add(nameof(Invoice.InvoiceNumber), typeof(string));
        invoiceTable.Columns.Add(nameof(Invoice.IssueDate), typeof(DateTime));
        invoiceTable.Columns.Add(nameof(Invoice.DueDate), typeof(DateTime));
        invoiceTable.Columns.Add(nameof(Invoice.Amount), typeof(string));
        invoiceTable.Columns.Add(nameof(Invoice.CurrencyCode), typeof(string));
        invoiceTable.Columns.Add(nameof(Invoice.PaymentStatus), typeof(string));

        int counter = 1;
        foreach (var invoice in invoices)
        {
            DataRow invoiceRecord = invoiceTable.NewRow();
            string amount = currencyExchangeService.GetAmountWithSymbol(invoice.Amount, invoice.CurrencyCode);

            invoiceRecord["No"] = counter;
            invoiceRecord[nameof(Invoice.InvoiceNumber)] = invoice.InvoiceNumber.ToString("D6");
            invoiceRecord[nameof(Invoice.IssueDate)] = invoice.IssueDate;
            invoiceRecord[nameof(Invoice.DueDate)] = invoice.DueDate;
            invoiceRecord[nameof(Invoice.Amount)] = amount;
            invoiceRecord[nameof(Invoice.CurrencyCode)] = invoice.CurrencyCode;
            invoiceRecord[nameof(Invoice.PaymentStatus)] = invoice.PaymentStatus.ToString();
            counter++;

            invoiceTable.Rows.Add(invoiceRecord);
        }

        ImportTableOptions importOptions = new ImportTableOptions();

        Worksheet dataTableWorksheet = workbook.Worksheets[0];
        dataTableWorksheet.Name = "Invoice";
        dataTableWorksheet.Cells.ImportData(invoiceTable, 0, 0, importOptions);

        var cells = dataTableWorksheet.Cells;
        var headerStyle = cells["A1"].GetStyle();
        headerStyle.HorizontalAlignment = TextAlignmentType.Center;
        headerStyle.VerticalAlignment = TextAlignmentType.Center;
        headerStyle.Font.IsBold = true;
        headerStyle.ForegroundColor = Color.MediumSlateBlue;
        headerStyle.Pattern = BackgroundType.Solid;
        headerStyle.Font.Color = Color.White;

        for (int col = 0; col < invoiceTable.Columns.Count; col++)
        {
            cells[0, col].SetStyle(headerStyle);
            cells.SetColumnWidth(col, 18);
        }

        for (int row = 1; row <= invoiceTable.Rows.Count; row++)
        {
            for (int col = 0; col < invoiceTable.Columns.Count; col++)
            {
                Cell currentCell = cells[row, col];
                Style cellStyle = currentCell.GetStyle();

                cellStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                cellStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                cellStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                cellStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

                if (row % 2 == 0)
                {
                    cellStyle.BackgroundColor = Color.WhiteSmoke;
                }
                else
                {
                    cellStyle.BackgroundColor = Color.White;
                }

                cellStyle.Pattern = BackgroundType.Solid;

                if (col == 0 || invoiceTable.Columns[col].ColumnName == "Amount")
                {
                    cellStyle.HorizontalAlignment = TextAlignmentType.Right;
                }

                if (invoiceTable.Columns[col].ColumnName == "PaymentStatus")
                {
                    string status = currentCell.StringValue;
                    if (status == "Paid")
                    {
                        cellStyle.Font.Color = Color.ForestGreen;
                    }
                    else if (status == "Unpaid")
                    {
                        cellStyle.Font.Color = Color.Crimson;
                    }
                }

                currentCell.SetStyle(cellStyle);
            }
        }


        dataTableWorksheet.FreezePanes(1, 0, 1, invoiceTable.Columns.Count);
        dataTableWorksheet.AutoFitColumns();

        workbook.Save(@"C:\Users\Abbos\OneDrive\Desktop\output.xlsx", SaveFormat.Xlsx);
    }
}
