using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External;
using Application.Common.Interfaces.Repositories;

namespace Application.Customers.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<CustomerDto>, IClientRequest
{
    public UpdateCustomerCommand(Guid clientId, UpdateCustomerRequest request)
    {
        ClientId = clientId;
        Customer = request;
    }

    public UpdateCustomerRequest Customer { get; set; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdateCustomerCommandHandler(
    IMapper mapper,
    IClientRepository clientRepository,
    ICountryApiService countryApiService,
    ICustomerRepository customerRepository,
    IValidator<UpdateCustomerRequest> validator)
    : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Customer, cancellationToken: cancellationToken);

        _ = await countryApiService.GetByCode(request.Customer.CountryCode)
            ?? throw new NotFoundException($"Country is not found with this code: {request.Customer.CountryCode}" +
                                           $"Look at this https://www.iban.com/country-codes");

        _ = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var customer =
            await customerRepository.GetAsync(customer =>
                customer.ClientId == request.ClientId && customer.Id == request.Customer.Id)
            ?? throw new NotFoundException($"Customer is not found with this id: {request.Customer.Id}");

        mapper.Map(request.Customer, customer);
        customer.ClientId = request.ClientId;

        await customerRepository.UpdateAsync(customer);

        return mapper.Map<CustomerDto>(customer);
    }
}