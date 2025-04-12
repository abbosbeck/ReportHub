using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Clients.CreateClient;

public class CreateClientCommand : IRequest<ClientDto>
{
    public string Name { get; set; }

    public Guid OwnerId { get; set; }
}

public class CreateClientCommandHandler(
    IClientRepository clientRepository,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository,
    IClientRoleRepository clientRoleRepository,
    IUserRepository userRepository,
    IValidator<CreateClientCommand> validator,
    IMapper mapper)
    : IRequestHandler<CreateClientCommand, ClientDto>
{
    public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var owner = await userRepository.GetByIdAsync(request.OwnerId)
            ?? throw new NotFoundException($"User is not found with this id: {request.OwnerId}");

        var client = await clientRepository.AddAsync(new Client() { Name = request.Name });

        await AssignClientRoleAsync(client, owner.Id, ClientRoles.Owner);

        var clientDto = mapper.Map<ClientDto>(client);

        return clientDto;
    }

    private async Task AssignClientRoleAsync(
        Client client, Guid ownerId, string roleName)
    {
        var clientRole = await clientRoleRepository.GetByNameAsync(roleName)
            ?? throw new NotFoundException("Role not found!");

        var clientRoleAssignment = new ClientRoleAssignment
        {
            ClientId = client.Id,
            ClientRoleId = clientRole.Id,
            UserId = ownerId,
        };
        await clientRoleAssignmentRepository.AddAsync(clientRoleAssignment);
    }
}