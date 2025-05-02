using Application.ExportReports.ExportReportsToFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
[AllowAnonymous]
public class ExportReportsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetFileAsync(
        [FromRoute] Guid clientId,
        [FromQuery, BindRequired] ExportReportsFileType type,
        [FromQuery] ExportReportsReportType reportType )
    {
        var result = await Mediator.Send(new ExportReportsToFileQuery(clientId, type, reportType));

        return File(result.ByteArray, result.ContentType, result.FileName);
    }
}
