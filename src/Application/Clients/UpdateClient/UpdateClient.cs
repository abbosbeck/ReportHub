using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.Repositories;

namespace Application.Clients.UpdateClient;

public class UpdateClientCommand : IRequest<ClientDto>, IClientRequest
{
    public Guid ClientId { get; set; }

    public string Name { get; init; }

    public string CountryCode { get; init; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdateClientCommandHandler(
    IMapper mapper,
    IClientRepository repository,
    ICountryService countryService,
    IValidator<UpdateClientCommand> validator)
    : IRequestHandler<UpdateClientCommand, ClientDto>
{
    public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        _ = await countryService.GetByCodeAsync(request.CountryCode)
            ?? throw new NotFoundException($"Country is not found with this code: {request.CountryCode}." +
                                           $"Look at this https://www.iban.com/country-codes");

        var client = await repository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        mapper.Map(request, client);

        await repository.UpdateAsync(client);

        return mapper.Map<ClientDto>(client);
    }
}
