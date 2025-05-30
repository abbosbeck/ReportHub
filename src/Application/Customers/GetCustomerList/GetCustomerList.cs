﻿using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.GetCustomerList;

public class GetCustomerListQuery(Guid clientId) : IRequest<List<CustomerDto>>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetCustomerListQueryHandler(IMapper mapper, ICustomerRepository repository)
    : IRequestHandler<GetCustomerListQuery, List<CustomerDto>>
{
    public async Task<List<CustomerDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var customers = await repository
            .GetAll()
            .ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<List<CustomerDto>>(customers);
    }
}