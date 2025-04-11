using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Clients.RegisterClient;

public class RegisterClientCommand : IRequest<ClientDto>
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}

[RequiresSystemRole(SystemUserRoles.SystemAdmin)]
public class RegisterClientCommandHandler(
    IClientRepository repository,
    IValidator<RegisterClientCommand> validator,
    IPasswordHasher<Client> passwordHasher,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<RegisterClientCommand, ClientDto>
{
    public async Task<ClientDto> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var existClient = repository.IsClientExists(request.Email);
        if (existClient)
        {
            throw new ConflictException("There is a client with this email");
        }

        var client = mapper.Map<Client>(request);
        client.Password = passwordHasher.HashPassword(client, client.Password);

        var freshClient = await repository.AddClientAdminAsync(client, currentUser.UserId);

        return mapper.Map<ClientDto>(freshClient);
    }
}
