using Application.Reports.ExportReportsToFile.Request;

namespace Application.Reports.ExportReportsToFile;

public interface IExportReportsToFileQueryHandler
{
    Task<ExportReportsToFileDto> Handle(ExportReportsToFileQuery request, CancellationToken cancellationToken);
}
