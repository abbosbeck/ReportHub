using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Customers.GetCustomerById;

public class GetCustomerByIdQuery : IRequest<CustomerDto>, IClientRequest
{
    public Guid Id { get; init; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetCustomerByIdQueryHandler(IMapper mapper, ICustomerRepository repository)
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetAsync(customer => customer.ClientId == request.ClientId && customer.Id == request.Id)
            ?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

        return mapper.Map<CustomerDto>(customer);
    }
}