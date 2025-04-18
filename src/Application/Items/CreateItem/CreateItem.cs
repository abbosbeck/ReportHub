using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.CreateItem;

public class CreateItemCommand : IRequest<ItemDto>, IClientRequest
{
    public CreateItemCommand(Guid clientId, CreateItemRequest item)
    {
        ClientId = clientId;
        Item = item;
    }

    public CreateItemRequest Item { get; set; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class CreateItemCommandHandler(
    IItemRepository repository,
    IMapper mapper,
    IValidator<CreateItemCommand> validator)
    : IRequestHandler<CreateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var newItem = mapper.Map<Item>(request.Item);
        newItem.ClientId = request.ClientId;

        var createdItem = await repository.AddAsync(newItem)
            ?? throw new BadRequestException("Item creation failed!");

        return mapper.Map<ItemDto>(createdItem);
    }
}
