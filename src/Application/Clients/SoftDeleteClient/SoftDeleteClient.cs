using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces;

namespace Application.Clients.SoftDeleteClient;

public class SoftDeleteClientCommand : IRequest<bool>
{
    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientUserRoles.ClientAdmin)]
public class SoftDeleteClientCommandHandler(IClientRepository repository) : IRequestHandler<SoftDeleteClientCommand, bool>
{
    public async Task<bool> Handle(SoftDeleteClientCommand request, CancellationToken cancellationToken)
    {
        return await repository.SoftDeleteClientAsync(request.ClientId);
    }
}
