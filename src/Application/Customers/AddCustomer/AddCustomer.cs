using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Customers.AddCustomer;

public class AddCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string Country { get; init; }

    public Guid ClientId { get; init; }
}

public class AddCustomerCommandHandler(
    IMapper mapper,
    ICustomerRepository repository,
    IValidator<AddCustomerCommand> validator)
    : IRequestHandler<AddCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        var result = await repository.AddAsync(customer);

        return mapper.Map<CustomerDto>(result);
    }
}