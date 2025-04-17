using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;

namespace Application.Customers.GetCustomerById;

public class GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public Guid Id { get; init; }
}

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