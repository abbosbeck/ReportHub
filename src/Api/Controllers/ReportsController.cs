using System.ComponentModel.DataAnnotations;
using Application.Reports.ExportReportsToFile;
using Application.Reports.ExportReportsToFile.Request;
using Application.Reports.GetCurrentUserReportSchedule;
using Application.Reports.ReScheduleReport;
using Application.Reports.ScheduleReport;
using Application.Reports.StopReportSchedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
public class ReportsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleAsync([FromRoute] Guid clientId, [FromBody] ScheduleReportRequest request)
    {
        var result = await Mediator.Send(new ScheduleReportCommand(clientId, request));

        return Ok(result);
    }

    [HttpPut("re-schedule")]
    public async Task<IActionResult> ReScheduleAsync([FromRoute] Guid clientId, [FromBody] ReScheduleReportRequest request)
    {
        var result = await Mediator.Send(new ReScheduleReportCommand(clientId, request));

        return Ok(result);
    }

    [HttpGet("download-report")]
    public async Task<IActionResult> GetFileAsync(
        [FromRoute] Guid clientId,
        [FromQuery, Required] ExportReportsFileType fileType,
        [FromQuery] ExportReportsReportTableType? reportTableType)
    {
        var result = await Mediator.Send(new ExportReportsToFileQuery(clientId, fileType, reportTableType));

        return File(result.ByteArray, result.ContentType, result.FileName);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new GetCurrentUserReportScheduleQuery(clientId));

        return Ok(result);
    }

    [HttpDelete("stop")]
    public async Task<IActionResult> StopAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new StopReportScheduleCommand(clientId));

        return Ok(result);
    }
}