using Application.ExportReports.ExportReportsToFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
[AllowAnonymous]
public class ExportReportsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetFileAsync([FromRoute] Guid clientId, [FromQuery] ExportReportsFileType type)
    {
        var result = await Mediator.Send(new ExportReportsToFileQuery(clientId, type));

        return Ok(result);
    }
}
