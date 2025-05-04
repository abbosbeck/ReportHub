using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.ReportSchedules.GetCurrentUserReportSchedule;

public class GetCurrentUserReportScheduleQuery(Guid clientId)
    : IRequest<ReportScheduleDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Operator, ClientRoles.ClientAdmin, ClientRoles.Owner)]
public class GetCurrentUserReportScheduleQueryHandler(
    IMapper mapper,
    ICurrentUserService service,
    IReportScheduleRepository repository)
    : IRequestHandler<GetCurrentUserReportScheduleQuery, ReportScheduleDto>
{
    public async Task<ReportScheduleDto> Handle(GetCurrentUserReportScheduleQuery request, CancellationToken cancellationToken)
    {
        var reportSchedule = await repository.GetByUserIdAsync(service.UserId)
            ?? throw new NotFoundException("Report Schedule is not found");

        return mapper.Map<ReportScheduleDto>(reportSchedule);
    }
}