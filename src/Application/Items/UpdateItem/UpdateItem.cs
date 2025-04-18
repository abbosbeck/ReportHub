using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.UpdateItem;

public class UpdateItemCommand : IRequest<ItemDto>, IClientRequest
{
    public UpdateItemCommand(Guid clientId, UpdateItemRequest item)
    {
        Item = item;
        ClientId = clientId;
    }

    public UpdateItemRequest Item { get; set; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdateItemCommandHandler(
    IItemRepository repository,
    IMapper mapper,
    IValidator<UpdateItemRequest> validator)
    : IRequestHandler<UpdateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

        _ = await repository.GetByIdAsync(request.Item.Id)
            ?? throw new NotFoundException($"Item is not found with this id: {request.Item.Id}");

        var newItem = mapper.Map<Item>(request.Item);
        newItem.ClientId = request.ClientId;

        var updatedItem = await repository.UpdateAsync(newItem);

        return mapper.Map<ItemDto>(updatedItem);
    }
}
