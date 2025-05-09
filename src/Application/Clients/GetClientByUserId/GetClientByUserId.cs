
using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Clients.GetClientByUserId;

public class GetClientByUserIdQuery : IRequest<List<ClientDto>>
{
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetClientByUserIdQueryHandler(
    ICurrentUserService userService,
    IClientRepository clientRepository,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository,
    IMapper mapper)
    : IRequestHandler<GetClientByUserIdQuery, List<ClientDto>>
{
    public async Task<List<ClientDto>> Handle(GetClientByUserIdQuery request, CancellationToken cancellationToken)
    {
        var clientRoles = await clientRoleAssignmentRepository.GetAll()
            .Where(c => c.UserId == userService.UserId)
            .Select(c => c.ClientId)
            .ToListAsync(cancellationToken);

        var clients = await clientRepository.GetAll()
            .Where(c => clientRoles.Contains(c.Id))
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ClientDto>>(clients);
    }
}
