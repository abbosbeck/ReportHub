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

public class RegisterClientCommandHandler(
    IClientRepository clientRepository,
    IValidator<RegisterClientCommand> validator,
    IPasswordHasher<Client> passwordHasher,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<RegisterClientCommand, ClientDto>
{
    public async Task<ClientDto> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var existClient = await clientRepository.GetClientByEmailAsync(request.Email);
        if (existClient is not null)
        {
            throw new ConflictException("There is a client with this email");
        }

        var client = mapper.Map<Client>(request);
        client.Password = passwordHasher.HashPassword(client, client.Password);

        var freshClient = await clientRepository.AddClientAdminAsync(client, currentUser.UserId);

        return mapper.Map<ClientDto>(freshClient);
    }
}
