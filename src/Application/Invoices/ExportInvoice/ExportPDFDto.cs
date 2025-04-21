namespace Application.Invoices.ExportInvoice;

public class ExportPDFDto
{
    public byte[] ByteArray { get; init; }

    public string ContentType { get; init; }

    public string FileName { get; init; }
}