using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Clients.CreateClient;

public class CreateClientCommand : IRequest<ClientDto>
{
    public string Name { get; set; }

    public string CountryCode { get; set; }

    public Guid OwnerId { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class CreateClientCommandHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IClientRepository clientRepository,
    ICountryService countryService,
    IValidator<CreateClientCommand> validator,
    IClientRoleRepository clientRoleRepository,
    IClientRoleAssignmentRepository clientRoleAssignmentRepository)
    : IRequestHandler<CreateClientCommand, ClientDto>
{
    public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        _ = await countryService.GetByCodeAsync(request.CountryCode)
            ?? throw new NotFoundException($"Country is not found with this code: {request.CountryCode}." +
                                           $"Look at this https://www.iban.com/country-codes");

        var owner = await userRepository.GetByIdAsync(request.OwnerId)
            ?? throw new NotFoundException($"User is not found with this id: {request.OwnerId}");

        var client = mapper.Map<Client>(request);
        await clientRepository.AddAsync(client);

        await AssignClientRoleAsync(client, owner.Id, ClientRoles.Owner);

        return mapper.Map<ClientDto>(client);
    }

    private async Task AssignClientRoleAsync(Client client, Guid ownerId, string roleName)
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