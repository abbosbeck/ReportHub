using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Items.DeleteItem;

public class DeleteItemCommand(Guid itemId, Guid clientId) : IRequest<bool>, IClientRequest
{
    public Guid ItemId { get; init; } = itemId;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class DeleteItemCommandHandler(IItemRepository repository) : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.ItemId)
            ?? throw new NotFoundException($"Item is not found with this id: {request.ClientId}");

        var isDeleted = await repository.DeleteAsync(item);

        return isDeleted;
    }
}
