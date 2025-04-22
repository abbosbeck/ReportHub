using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Invoices.GetExportLogById;

public class GetExportLogByIdQuery(Guid clientId, string logId) : IRequest<LogDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;

    public string LogId { get; set; } = logId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetExportLogByIdQueryHandler(IMapper mapper, ILogRepository logRepository)
    : IRequestHandler<GetExportLogByIdQuery, LogDto>
{
    public async Task<LogDto> Handle(GetExportLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = logRepository.GetById(request.LogId)
            ?? throw new NotFoundException($"Log is not found with this Id: {request.LogId}");

        return mapper.Map<LogDto>(log);
    }
}