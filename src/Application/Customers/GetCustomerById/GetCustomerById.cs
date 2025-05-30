﻿using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Customers.GetCustomerById;

public class GetCustomerByIdQuery(Guid id, Guid clientId) : IRequest<CustomerDto>, IClientRequest
{
    public Guid Id { get; init; } = id;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetCustomerByIdQueryHandler(IMapper mapper, ICustomerRepository repository)
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

        return mapper.Map<CustomerDto>(customer);
    }
}