using Application.ExportReports.ExportReportsToFile.Request;

namespace Application.ExportReports.ExportReportsToFile;

public interface IExportReportsToFileQueryHandler
{
    Task<ExportReportsToFileDto> Handle(ExportReportsToFileQuery request, CancellationToken cancellationToken);
}
