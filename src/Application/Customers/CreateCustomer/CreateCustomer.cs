using Application.Common.Exceptions;
using Application.Common.Interfaces.External;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Customers.CreateCustomer;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string CountryCode { get; init; }

    public Guid ClientId { get; init; }
}

public class CreateCustomerCommandHandler(
    IMapper mapper,
    IClientRepository clientRepository,
    ICountryApiService countryApiService,
    ICustomerRepository customerRepository,
    IValidator<CreateCustomerCommand> validator)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        _ = await countryApiService.GetByCode(request.CountryCode)
            ?? throw new NotFoundException($"Country is not found with this code: {request.CountryCode}" +
                                           $"Look at this https://www.iban.com/country-codes");

        _ = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var customer = mapper.Map<Customer>(request);
        await customerRepository.AddAsync(customer);

        return mapper.Map<CustomerDto>(customer);
    }
}