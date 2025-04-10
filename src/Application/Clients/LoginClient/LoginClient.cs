using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Clients.LoginClient;

public class LoginClientCommand : IRequest<LoginDto>
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class LoginClientCommandHandler(
    IClientRepository clientRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher<Client> passwordHasher,
    IValidator<LoginClientCommand> validator)
    : IRequestHandler<LoginClientCommand, LoginDto>
{
    public async Task<LoginDto> Handle(LoginClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var client = await clientRepository.GetClientByEmailAsync(request.Email)
            ?? throw new UnauthorizedException("Invalid email or password");
        var verifyPassword = passwordHasher.VerifyHashedPassword(client, client.Password, request.Password);
        if (verifyPassword == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        var token = await jwtTokenGenerator.GenerateAccessTokenAsync(client);

        return new LoginDto
        {
            AccessToken = token,
        };
    }
}