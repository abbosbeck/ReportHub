using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Aspose.Cells;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Services;

public class ImportDataFromFileServic : IImportDataFromFileService
{
    public List<Customer> ImportCustomerListFromExcel(IFormFile file)
    {
        Workbook workbook = ReadExcelDataFromIFormFile(file);
        Worksheet worksheet = workbook.Worksheets[0];
        var customers = new List<Customer>();

        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++)
        {
            Customer customer = new Customer();

            if (worksheet.Cells[row, 1].Value != null)
            {
                customer.Name = worksheet.Cells[row, 1].Value.ToString();
            }

            if (worksheet.Cells[row, 2].Value != null)
            {
                customer.Email = worksheet.Cells[row, 2].Value.ToString();
            }

            if (worksheet.Cells[row, 3].Value != null)
            {
                customer.CountryCode = worksheet.Cells[row, 3].Value.ToString();
            }

            customers.Add(customer);
        }

        return customers;
    }

    private static Workbook ReadExcelDataFromIFormFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        string extension = Path.GetExtension(file.FileName);
        if (!(string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(extension, ".xls", StringComparison.OrdinalIgnoreCase)))
        {
            throw new BadRequestException("You have to provide an Excel file!");
        }

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                memoryStream.Position = 0;

                Workbook workbook = new Workbook(memoryStream);

                return workbook;
            }
        }
        catch (CellsException ex)
        {
            throw new BadRequestException($"An error occured while reading file: {ex}");
        }
        catch
        {
            throw new BadRequestException("Something went wrong!");
        }
    }
}
