using Application.ReportSchedules.GetCurrentUserReportSchedule;
using Application.ReportSchedules.ReScheduleReport;
using Application.ReportSchedules.ScheduleReport;
using Application.ReportSchedules.StopReportSchedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
public class ReportSchedulesController(ISender mediator) : ApiControllerBase(mediator)
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

    [HttpGet("me")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new GetCurrentUserReportScheduleQuery(clientId));

        return Ok(result);
    }

    [HttpGet("stop")]
    public async Task<IActionResult> StopAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new StopReportScheduleCommand(clientId));

        return Ok(result);
    }
}