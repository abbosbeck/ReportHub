using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;

namespace Application.Clients.DeleteClient;

public class DeleteClientCommand : IRequest<bool>
{
    public Guid ClientId { get; set; }
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

        client.IsDeleted = true;
        client.DeletedOn = DateTime.UtcNow;
        await clientRepository.UpdateAsync(client);

        return true;
    }
}