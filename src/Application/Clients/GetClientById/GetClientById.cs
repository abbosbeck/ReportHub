using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Clients.GetClientById;

public class GetClientByIdQuery(Guid clientId) : IRequest<ClientDto>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetClientByIdQueryHandler(IMapper mapper, IClientRepository repository)
    : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        return mapper.Map<ClientDto>(client);
    }
}
