using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.UpdateItem;

public class UpdateItemCommand(Guid clientId, UpdateItemRequest item) : IRequest<ItemDto>, IClientRequest
{
    public UpdateItemRequest Item { get; set; } = item;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdateItemCommandHandler(
    IMapper mapper,
    IItemRepository repository,
    IValidator<UpdateItemRequest> validator)
    : IRequestHandler<UpdateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

        _ = await repository.GetByIdAsync(request.Item.Id)
            ?? throw new NotFoundException($"Item is not found with this id: {request.Item.Id}");

        var item = mapper.Map<Item>(request.Item);
        item.ClientId = request.ClientId;

        await repository.UpdateAsync(item);

        return mapper.Map<ItemDto>(item);
    }
}
