using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Clients.GetClientsList;

public class GetClientsListQuery : IRequest<IEnumerable<ClientDto>>
{
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class GetClientsListQueryHandler(IMapper mapper, IClientRepository repository)
    : IRequestHandler<GetClientsListQuery, IEnumerable<ClientDto>>
{
    public async Task<IEnumerable<ClientDto>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        var clients = await repository.GetAll().ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<IEnumerable<ClientDto>>(clients);
    }
}