using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Clients.DeleteClient;

public class DeleteClientCommand(Guid clientId) : IRequest<bool>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class DeleteClientCommandHandler(
    IClientRepository clientRepository,
    IValidator<DeleteClientCommand> validator)
    : IRequestHandler<DeleteClientCommand, bool>
{
    public async Task<bool> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var result = await clientRepository.DeleteAsync(client);

        return result;
    }
}