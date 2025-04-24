using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Invoices.GetExportLogById;

public class GetExportLogByIdQuery(Guid clientId, Guid logId) : IRequest<LogDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;

    public Guid LogId { get; set; } = logId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetExportLogByIdQueryHandler(IMapper mapper, ILogRepository logRepository)
    : IRequestHandler<GetExportLogByIdQuery, LogDto>
{
    public async Task<LogDto> Handle(GetExportLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await logRepository.GetByIdAsync(request.LogId, request.ClientId)
            ?? throw new NotFoundException($"Log is not found with this Id: {request.LogId}");

        return mapper.Map<LogDto>(log);
    }
}