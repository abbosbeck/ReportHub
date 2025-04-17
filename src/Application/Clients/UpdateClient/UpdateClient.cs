using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Clients.UpdateClient;

public class UpdateClientCommand : IRequest<ClientDto>, IClientRequest
{
    public Guid ClientId { get; set; }

    public string Name { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
[RequiresClientRole(ClientRoles.Owner)]
public class UpdateClientCommandHandler(
    IClientRepository repository,
    IMapper mapper,
    ICurrentUserService currentUser,
    IValidator<UpdateClientCommand> validator)
    : IRequestHandler<UpdateClientCommand, ClientDto>
{
    public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var client = await repository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        client.Name = request.Name;
        client.LastModifiedOn = DateTime.UtcNow;
        client.LastModifiedBy = currentUser.UserId.ToString();

        var updatedClient = await repository.UpdateAsync(client);

        return mapper.Map<ClientDto>(updatedClient);
    }
}
