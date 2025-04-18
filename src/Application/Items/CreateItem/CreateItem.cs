using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.CreateItem;

public class CreateItemCommand(Guid clientId, CreateItemRequest item) : IRequest<ItemDto>, IClientRequest
{
    public CreateItemRequest Item { get; set; } = item;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class CreateItemCommandHandler(
    IMapper mapper,
    IItemRepository repository,
    IValidator<CreateItemRequest> validator)
    : IRequestHandler<CreateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

        var item = mapper.Map<Item>(request.Item);
        item.ClientId = request.ClientId;

        await repository.AddAsync(item);

        return mapper.Map<ItemDto>(item);
    }
}
