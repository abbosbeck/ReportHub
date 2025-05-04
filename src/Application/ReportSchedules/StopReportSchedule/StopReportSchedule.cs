using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.ReportSchedules.StopReportSchedule;

public class StopReportScheduleCommand(Guid clientId) : IRequest<bool>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

public class StopReportScheduleCommandHandler(
    IReportScheduleRepository repository,
    ICurrentUserService currentUserService,
    IReportScheduleService reportScheduleService) : IRequestHandler<StopReportScheduleCommand, bool>
{
    public async Task<bool> Handle(StopReportScheduleCommand request, CancellationToken cancellationToken)
    {
        var reportSchedule = await repository.GetByUserIdAsync(currentUserService.UserId)
            ?? throw new NotFoundException("Report Schedule is not found");

        await reportScheduleService.DeleteAsync(reportSchedule);
        var result = await repository.DeleteAsync(reportSchedule);

        return result;
    }
}