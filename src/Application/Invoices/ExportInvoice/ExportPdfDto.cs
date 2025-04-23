namespace Application.Invoices.ExportInvoice;

public class ExportPdfDto
{
    public byte[] ByteArray { get; init; }

    public string ContentType { get; init; }

    public string FileName { get; init; }
}