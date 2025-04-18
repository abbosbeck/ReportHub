using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Customers.DeleteCustomer;

public class DeleteCustomerCommand(Guid id, Guid clientId) : IRequest<bool>, IClientRequest
{
    public Guid Id { get; init; } = id;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner)]
public class DeleteCustomerCommandHandler(ICustomerRepository repository)
    : IRequestHandler<DeleteCustomerCommand, bool>
{
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetAsync(
               customer => customer.ClientId == request.ClientId && customer.Id == request.Id)
           ?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

        var result = await repository.DeleteAsync(customer);

        return result;
    }
}