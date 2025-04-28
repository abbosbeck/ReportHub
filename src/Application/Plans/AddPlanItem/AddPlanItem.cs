using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Plans.AddPlanItem;

public class AddPlanItemCommand(Guid planId, AddPlanItemRequest addPlanItem, Guid clientId)
    : IRequest<PlanItemDto>, IClientRequest
{
    public Guid PlanId { get; init; } = planId;

    public AddPlanItemRequest AddPlanItem { get; init; } = addPlanItem;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class AddPlanItemCommandHandler(
    IMapper mapper,
    IPlanRepository planRepository,
    IItemRepository itemRepository,
    IPlanItemRepository planItemRepository,
    IValidator<AddPlanItemRequest> validator)
    : IRequestHandler<AddPlanItemCommand, PlanItemDto>
{
    public async Task<PlanItemDto> Handle(AddPlanItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.AddPlanItem, cancellationToken: cancellationToken);

        var plan = await planRepository.GetByIdAsync(request.PlanId)
            ?? throw new NotFoundException($"Plan is not found with this id: {request.PlanId}");

        var item = await itemRepository.GetByIdAsync(request.AddPlanItem.ItemId)
            ?? throw new NotFoundException($"Item is not found with this id: {request.AddPlanItem.ItemId}");

        var planItem = new PlanItem
        {
            ItemId = item.Id,
            Quantity = request.AddPlanItem.Quantity,
            PlanId = plan.Id,
        };

        await planItemRepository.AddAsync(planItem);

        return mapper.Map<PlanItemDto>(planItem);
    }
}