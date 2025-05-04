namespace Application.Reports.ExportReportsToFile.Request;

public class ExportReportsToFileDto(byte[] byteArray, string contentType, string fileName)
{
    public byte[] ByteArray { get; set; } = byteArray;

    public string ContentType { get; set; } = contentType;

    public string FileName { get; set; } = fileName;
}