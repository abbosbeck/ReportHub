using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Customers.DeleteCustomer;

public class DeleteCustomerCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<DeleteCustomerCommand, bool>
{
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerById(request.Id)
            ?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

        return true;
    }
}