using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Customers.CreateCustomer;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string Country { get; init; }
}

public class CreateCustomerCommandHandler(
    ICustomerRepository repository,
    IValidator<CreateCustomerCommand> validator,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var isExistCustomer = repository.CheckIsCustomerExistByEmail(request.Email);
        if (isExistCustomer)
        {
            throw new ConflictException("There is customer with this email!");
        }

        var newCustomer = mapper.Map<Customer>(request);
        newCustomer.ClientId = currentUser.UserId;

        await repository.AddCustomerAsync(newCustomer);
        var createdCustomer = await repository.GetCustomerByEmail(newCustomer.Email);
        var customerDto = mapper.Map<CustomerDto>(createdCustomer);

        return customerDto;
    }
}