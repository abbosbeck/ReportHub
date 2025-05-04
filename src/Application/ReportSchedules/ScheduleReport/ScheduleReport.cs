using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.ReportSchedules.ScheduleReport;

public class ScheduleReportCommand(Guid clientId, ScheduleReportRequest request)
    : IRequest<ReportScheduleDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;

    public ScheduleReportRequest ReportSchedule { get; init; } = request;
}

[RequiresClientRole(ClientRoles.Operator, ClientRoles.ClientAdmin, ClientRoles.Owner)]
public class ScheduleReportCommandHandler(
    IMapper mapper,
    IReportScheduleRepository repository,
    IReportScheduleService scheduleService,
    ICurrentUserService currentUserService,
    IValidator<ScheduleReportRequest> validator)
    : IRequestHandler<ScheduleReportCommand, ReportScheduleDto>
{
    public async Task<ReportScheduleDto> Handle(ScheduleReportCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.ReportSchedule, cancellationToken);

        var reportSchedule = mapper.Map<ReportSchedule>(request.ReportSchedule);
        reportSchedule.UserId = currentUserService.UserId;

        await scheduleService.ScheduleAsync(reportSchedule);
        await repository.AddAsync(reportSchedule);

        return mapper.Map<ReportScheduleDto>(reportSchedule);
    }
}