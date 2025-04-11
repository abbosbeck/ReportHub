using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Customers.AddCustomer;

public class AddCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string Country { get; init; }
}

public class AddCustomerCommandHandler(
    IMapper mapper,
    ICustomerRepository repository,
    ICurrentUserService currentUser,
    IValidator<AddCustomerCommand> validator)
    : IRequestHandler<AddCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var isExistCustomer = repository.CheckIsCustomerExistByEmail(request.Email);
        if (isExistCustomer)
        {
            throw new ConflictException("There is already a customer with this email!");
        }

        var newCustomer = mapper.Map<Customer>(request);
        newCustomer.ClientId = currentUser.UserId;

        await repository.AddCustomerAsync(newCustomer);
        var createdCustomer = await repository.GetCustomerByEmail(newCustomer.Email);
        var customerDto = mapper.Map<CustomerDto>(createdCustomer);

        return customerDto;
    }
}