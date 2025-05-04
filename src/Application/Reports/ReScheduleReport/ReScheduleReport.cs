using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Reports.ReScheduleReport;

public class ReScheduleReportCommand(Guid clientId, ReScheduleReportRequest reportSchedule) 
    : IRequest<ReportScheduleDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;

    public ReScheduleReportRequest ReportSchedule { get; init; } = reportSchedule;
}

[RequiresClientRole(ClientRoles.Operator, ClientRoles.ClientAdmin, ClientRoles.Owner)]
public class ReScheduleReportCommandHandler(
    IMapper mapper,
    IReportScheduleRepository repository,
    ICurrentUserService currentUserService,
    IReportScheduleService reportScheduleService,
    IValidator<ReScheduleReportRequest> validator)
    : IRequestHandler<ReScheduleReportCommand, ReportScheduleDto>
{
    public async Task<ReportScheduleDto> Handle(ReScheduleReportCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.ReportSchedule, cancellationToken);

        var reportSchedule = await repository.GetByUserIdAsync(currentUserService.UserId)
            ?? throw new NotFoundException("Report Schedule is not found");

        mapper.Map(request.ReportSchedule, reportSchedule);
        reportSchedule.ClientId = request.ClientId;

        await reportScheduleService.ReScheduleAsync(reportSchedule);
        await repository.UpdateAsync(reportSchedule);

        return mapper.Map<ReportScheduleDto>(reportSchedule);
    }
}