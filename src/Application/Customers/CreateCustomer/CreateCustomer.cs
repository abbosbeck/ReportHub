﻿using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Customers.CreateCustomer;

public class CreateCustomerCommand(Guid clientId, CreateCustomerRequest request)
    : IRequest<CustomerDto>, IClientRequest
{
    public CreateCustomerRequest Customer { get; set; } = request;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class CreateCustomerCommandHandler(
    IMapper mapper,
    ICountryService countryService,
    IClientRepository clientRepository,
    ICustomerRepository customerRepository,
    IValidator<CreateCustomerRequest> validator)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Customer, cancellationToken: cancellationToken);

        _ = await countryService.GetByCodeAsync(request.Customer.CountryCode)
            ?? throw new NotFoundException($"Country is not found with this code: {request.Customer.CountryCode}. " +
                                           $"Look at this https://www.iban.com/country-codes");

        _ = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var customer = mapper.Map<Customer>(request.Customer);
        customer.ClientId = request.ClientId;

        await customerRepository.AddAsync(customer);

        return mapper.Map<CustomerDto>(customer);
    }
}