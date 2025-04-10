using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Clients.GiveRoleToClientMember;

public class GiveRoleToClientMemberCommand : IRequest<bool>
{
    public Guid ClientId { get; set; }

    public string RoleName { get; set; }
}

[RequiresClientRole(ClientUserRoles.ClientAdmin)]
public class GiveRoleToClientMemberCommandHandler(
    IClientRepository repository)
    : IRequestHandler<GiveRoleToClientMemberCommand, bool>
{
    public async Task<bool> Handle(GiveRoleToClientMemberCommand request, CancellationToken cancellationToken)
    {
        var existClient = await repository.GetClientByIdAsync(request.ClientId);
        if (existClient is null)
        {
            throw new NotFoundException("User not found with this Id.");
        }

        var result = await repository.GiveRoleToClientMemberAsync(request.ClientId, request.RoleName);

        return result;
    }
}
