using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Clients.AddClientMember;

public class AddClientMemberCommand : IRequest<ClientDto>
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}

[RequiresClientRole(ClientUserRoles.ClientAdmin)]
public class AddClientMemberCommandHandler(
    IClientRepository repository,
    IMapper mapper,
    IPasswordHasher<Client> passwordHasher,
    IValidator<AddClientMemberCommand> validator)
    : IRequestHandler<AddClientMemberCommand, ClientDto>
{
    public async Task<ClientDto> Handle(AddClientMemberCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var existClient = await repository.GetClientByEmailAsync(request.Email);
        if (existClient is not null)
        {
            throw new ConflictException("There is a member with this email");
        }

        var client = mapper.Map<Client>(request);
        client.Password = passwordHasher.HashPassword(client, client.Password);

        await repository.AddClientMemberAsync(client);
        var freshClient = await repository.GetClientByEmailAsync(client.Email);

        return mapper.Map<ClientDto>(freshClient);
    }
}
