using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Plans.GetPlanById;

public class GetPlanByIdQuery : IRequest<PlanDto>, IClientRequest
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }
}

public class GetPlanByIdQueryHandler(
    IPlanRepository planRepository,
    IPlanItemRepository planItemRepository,
    IItemRepository itemRepository,
    IMapper mapper)
    : IRequestHandler<GetPlanByIdQuery, PlanDto>
{
    public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await planRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Plan is not found with this id: {request.ClientId}");

        var itemIds = await planItemRepository.GetItemIdsByPlanIdAsync(plan.Id);

        var items = await itemRepository.GetByIdsAsync(itemIds);
        plan.Items = (ICollection<Item>)items;

        return mapper.Map<PlanDto>(plan);
    }
}