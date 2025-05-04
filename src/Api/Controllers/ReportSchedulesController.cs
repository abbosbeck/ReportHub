using Application.ReportSchedules.GetCurrentUserReportSchedule;
using Application.ReportSchedules.ScheduleReport;
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

    [HttpGet("me")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new GetCurrentUserReportScheduleQuery(clientId));

        return Ok(result);
    }
}