using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.GetExportLogsList;

public class GetExportLogsListQuery : IRequest<IEnumerable<LogDto>>, IClientRequest
{
    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetExportLogsListQueryHandler(IMapper mapper, ILogRepository logRepository)
    : IRequestHandler<GetExportLogsListQuery, IEnumerable<LogDto>>
{
    public async Task<IEnumerable<LogDto>> Handle(GetExportLogsListQuery request, CancellationToken cancellationToken)
    {
        var logs = await logRepository.GetAll().ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<IEnumerable<LogDto>>(logs);
    }
}