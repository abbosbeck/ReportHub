using System.Text.Json.Serialization;
using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Items.GetItemById;

public class GetItemByIdQuery : IRequest<ItemDto>, IClientRequest
{
    public Guid ItemId { get; set; }

    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetItemByIdQueryHandler(
    IItemRepository repository,
    IMapper mapper)
    : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.ItemId)
            ?? throw new NotFoundException($"Item is not found with this id: {request.ItemId}");

        return mapper.Map<ItemDto>(item);
    }
}
