using System.Text.Json.Serialization;
using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.GetItemsList;

public class GetItemListQuery : IRequest<List<ItemDto>>, IClientRequest
{
    public Guid ClientId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetItemListQueryHandler(
    IItemRepository repository,
    IMapper mapper)
    : IRequestHandler<GetItemListQuery, List<ItemDto>>
{
    public async Task<List<ItemDto>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAll().ToListAsync(cancellationToken);

        return mapper.Map<List<ItemDto>>(items);
    }
}
