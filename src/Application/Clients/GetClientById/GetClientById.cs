using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Clients.GetClientById;

public class GetClientByIdQuery : IRequest<ClientDto>, IClientRequest
{
    public Guid ClientId { get; set; }
}

public class GetClientByIdQueryHandler(IClientRepository repository,
    IMapper mapper)
    : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"User is not found with this id: {request.ClientId}");

        return mapper.Map<ClientDto>(client);
    }
}
