using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Clients.AssignClientRole;

public class AssignClientRoleCommand : IRequest<bool>, IClientRequest
{
    public Guid ClientId { get; set; }

    public Guid UserId { get; set; }

    public string RoleName { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
[RequiresClientRole(ClientRoles.Owner)]
public class AssignClientRoleHandler(
    IClientRepository clientRepository,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository,
    IClientRoleRepository clientRoleRepository,
    IUserRepository userRepository,
    IValidator<AssignClientRoleCommand> validator)
    : IRequestHandler<AssignClientRoleCommand, bool>
{
    public async Task<bool> Handle(AssignClientRoleCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");
        var clientRole = await clientRoleRepository.GetByNameAsync(request.RoleName)
            ?? throw new NotFoundException($"Client role is not found with this name: {request.RoleName}");

        var clientRoleAssignment = new ClientRoleAssignment
        {
            ClientId = client.Id,
            ClientRoleId = clientRole.Id,
            UserId = user.Id,
        };
        await clientRoleAssignmentRepository.AddAsync(clientRoleAssignment);

        return true;
    }
}
