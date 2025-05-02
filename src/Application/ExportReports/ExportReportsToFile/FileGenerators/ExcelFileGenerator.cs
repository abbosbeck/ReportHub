using System.Data;
using System.Drawing;
using Application.Common.Interfaces.External.CurrencyExchange;
using Aspose.Cells;
using Domain.Entities;

namespace Application.ExportReports.ExportReportsToFile.FileGenerators;

public class ExcelFileGenerator(ICurrencyExchangeService currencyExchangeService)
{
    public void GenerateExcelFile(List<Invoice> invoices, List<Item> items, List<PlanDto> plans)
    {
        Workbook mainWorkbook = new Workbook();
        Workbook invoiceWorkbook = GenerateInvoice(invoices);
        Workbook itemWorkBook = GenerateItems(items);
        Workbook planWorkBook = GeneratePlans(plans);

        WorksheetCollection sheets = mainWorkbook.Worksheets;
        sheets[0].Copy(invoiceWorkbook.Worksheets[0]);
        sheets[0].Name = "Invoices";

        sheets.Add();
        sheets[1].Copy(itemWorkBook.Worksheets[0]);
        sheets[1].Name = "Items";

        sheets.Add();
        sheets[2].Copy(planWorkBook.Worksheets[0]);
        sheets[2].Name = "Plans";

        mainWorkbook.Save(@"C:\Users\Abbos\OneDrive\Desktop\output.xlsx", SaveFormat.Xlsx);
    }

    private static Workbook GeneratePlans(List<PlanDto> plans)
    {
        Workbook workbook = new Workbook();
        DataTable planTable = new DataTable("Item");

        planTable.Columns.Add("No", typeof(long));
        planTable.Columns.Add(nameof(PlanDto.Title), typeof(string));
        planTable.Columns.Add(nameof(PlanDto.StartDate), typeof(DateTime));
        planTable.Columns.Add(nameof(PlanDto.EndDate), typeof(DateTime));
        planTable.Columns.Add(nameof(PlanDto.TotalPrice), typeof(string));
        planTable.Columns.Add(nameof(PlanDto.CurrencyCode), typeof(string));

        int counter = 1;
        foreach (var plan in plans)
        {
            DataRow invoiceRecord = planTable.NewRow();

            invoiceRecord["No"] = counter;
            invoiceRecord[nameof(PlanDto.Title)] = plan.Title;
            invoiceRecord[nameof(PlanDto.StartDate)] = plan.StartDate;
            invoiceRecord[nameof(PlanDto.EndDate)] = plan.EndDate;
            invoiceRecord[nameof(PlanDto.TotalPrice)] = plan.TotalPrice;
            invoiceRecord[nameof(PlanDto.CurrencyCode)] = plan.CurrencyCode;
            counter++;

            planTable.Rows.Add(invoiceRecord);
        }

        ImportTableOptions importOptions = new ImportTableOptions();

        Worksheet dataTableWorksheet = workbook.Worksheets[0];
        dataTableWorksheet.Name = "Item";
        dataTableWorksheet.Cells.ImportData(planTable, 0, 0, importOptions);

        var cells = dataTableWorksheet.Cells;
        var headerStyle = cells["A1"].GetStyle();
        headerStyle.HorizontalAlignment = TextAlignmentType.Center;
        headerStyle.VerticalAlignment = TextAlignmentType.Center;
        headerStyle.Font.IsBold = true;
        headerStyle.ForegroundColor = Color.MediumSlateBlue;
        headerStyle.Pattern = BackgroundType.Solid;
        headerStyle.Font.Color = Color.White;

        for (int col = 0; col < planTable.Columns.Count; col++)
        {
            cells[0, col].SetStyle(headerStyle);
            cells.SetColumnWidth(col, 18);
        }

        for (int row = 1; row <= planTable.Rows.Count; row++)
        {
            for (int col = 0; col < planTable.Columns.Count; col++)
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

                currentCell.SetStyle(cellStyle);
            }
        }

        dataTableWorksheet.FreezePanes(1, 0, 1, planTable.Columns.Count);
        dataTableWorksheet.AutoFitColumns();

        return workbook;
    }

    private Workbook GenerateItems(List<Item> items)
    {
        Workbook workbook = new Workbook();
        DataTable itemTable = new DataTable("Item");

        itemTable.Columns.Add("No", typeof(long));
        itemTable.Columns.Add(nameof(Item.Name), typeof(string));
        itemTable.Columns.Add(nameof(Item.Description), typeof(string));
        itemTable.Columns.Add(nameof(Item.Price), typeof(string));
        itemTable.Columns.Add(nameof(Item.CurrencyCode), typeof(string));
        itemTable.Columns.Add("Invoice Number", typeof(string));

        int counter = 1;
        foreach (var item in items)
        {
            DataRow invoiceRecord = itemTable.NewRow();
            string price = currencyExchangeService.GetAmountWithSymbol(item.Price, item.CurrencyCode);

            invoiceRecord["No"] = counter;
            invoiceRecord[nameof(Item.Name)] = item.Name;
            invoiceRecord[nameof(Item.Description)] = item.Description;
            invoiceRecord[nameof(Item.Price)] = price;
            invoiceRecord[nameof(Item.CurrencyCode)] = item.CurrencyCode;
            invoiceRecord["Invoice Number"] = item.Invoice.InvoiceNumber.ToString("D6");
            counter++;

            itemTable.Rows.Add(invoiceRecord);
        }

        ImportTableOptions importOptions = new ImportTableOptions();

        Worksheet dataTableWorksheet = workbook.Worksheets[0];
        dataTableWorksheet.Name = "Item";
        dataTableWorksheet.Cells.ImportData(itemTable, 0, 0, importOptions);

        var cells = dataTableWorksheet.Cells;
        var headerStyle = cells["A1"].GetStyle();
        headerStyle.HorizontalAlignment = TextAlignmentType.Center;
        headerStyle.VerticalAlignment = TextAlignmentType.Center;
        headerStyle.Font.IsBold = true;
        headerStyle.ForegroundColor = Color.MediumSlateBlue;
        headerStyle.Pattern = BackgroundType.Solid;
        headerStyle.Font.Color = Color.White;

        for (int col = 0; col < itemTable.Columns.Count; col++)
        {
            cells[0, col].SetStyle(headerStyle);
            cells.SetColumnWidth(col, 18);
        }

        for (int row = 1; row <= itemTable.Rows.Count; row++)
        {
            for (int col = 0; col < itemTable.Columns.Count; col++)
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

                if (col == 0 || itemTable.Columns[col].ColumnName == "Price")
                {
                    cellStyle.HorizontalAlignment = TextAlignmentType.Right;
                }

                currentCell.SetStyle(cellStyle);
            }
        }

        dataTableWorksheet.FreezePanes(1, 0, 1, itemTable.Columns.Count);
        dataTableWorksheet.AutoFitColumns();

        return workbook;
    }

    private Workbook GenerateInvoice(List<Invoice> invoices)
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

        return workbook;
    }
}