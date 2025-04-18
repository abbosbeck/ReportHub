using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Items.UpdateItem;

public class UpdateItemCommand : IRequest<ItemDto>, IClientRequest
{
    public Guid Id { get; set; }
    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }

    public Guid ClientId { get; set; }

    public Guid InvoiceId { get; init; }
}

public class UpdateItemCommandHandler(
    IItemRepository repository,
    IMapper mapper,
    IValidator<UpdateItemCommand> validator)
    : IRequestHandler<UpdateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        _ = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Item is not found with this id: {request.Id}");

        var newItem = mapper.Map<Item>(request);
        var updatedItem = await repository.UpdateAsync(newItem);

        return mapper.Map<ItemDto>(updatedItem);
    }
}
