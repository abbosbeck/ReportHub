using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External;
using Application.Common.Interfaces.Repositories;

namespace Application.Customers.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<CustomerDto>, IClientRequest
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string CountryCode { get; init; }

    public Guid ClientId { get; set; }
}

public class UpdateCustomerCommandHandler(
    IMapper mapper,
    IClientRepository clientRepository,
    ICountryApiService countryApiService,
    ICustomerRepository customerRepository,
    IValidator<UpdateCustomerCommand> validator)
    : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        _ = await countryApiService.GetByCode(request.CountryCode)
            ?? throw new NotFoundException($"Country is not found with this code: {request.CountryCode}" +
                                           $"Look at this https://www.iban.com/country-codes");

        _ = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var customer = await customerRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

        mapper.Map(request, customer);

        await customerRepository.UpdateAsync(customer);

        return mapper.Map<CustomerDto>(customer);
    }
}