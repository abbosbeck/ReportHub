using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.CreateItem;

public class CreateItemCommand : IRequest<ItemDto>, IClientRequest
{
    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }

    public Guid ClientId { get; set; }

    public Guid InvoiceId { get; init; }
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
        await validator.ValidateAndThrowAsync(request);

        var newItem = mapper.Map<Item>(request);
        var createdItem = await repository.AddAsync(newItem)
            ?? throw new BadRequestException("Item creation failed!");

        return mapper.Map<ItemDto>(createdItem);
    }
}
