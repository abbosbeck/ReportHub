﻿using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Clients.AddClientMember;

public class AddClientMemberCommand : IRequest<bool>, IClientRequest
{
    public Guid ClientId { get; set; }

    public Guid UserId { get; set; }
 }

[RequiresSystemRole(SystemRoles.SuperAdmin)]
[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class AddClientMemberCommandHandler(
    IUserRepository userRepository,
    IClientRepository clientRepository,
    IClientRoleRepository clientRoleRepository,
    IValidator<AddClientMemberCommand> validator,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository)
    : IRequestHandler<AddClientMemberCommand, bool>
{
    public async Task<bool> Handle(AddClientMemberCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");

        var client = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var clientRole = await clientRoleRepository.GetByNameAsync(ClientRoles.Operator)
            ?? throw new NotFoundException($"Client role is not found with this name: {ClientRoles.Operator}");

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