using System.ComponentModel.DataAnnotations;
using Application.ExportReports.ExportReportsToFile;
using Application.ExportReports.ExportReportsToFile.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
public class ExportReportsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetFileAsync(
        [FromRoute] Guid clientId,
        [FromQuery, Required] ExportReportsFileType type,
        [FromQuery] ExportReportsReportTableType? reportTableType )
    {
        var result = await Mediator.Send(new ExportReportsToFileQuery(clientId, type, reportTableType));

        return File(result.ByteArray, result.ContentType, result.FileName);
    }
}
