using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Plans.DeletePlanItem;

public class DeletePlanItemCommand(Guid planId, Guid itemId, Guid clientId) : IRequest<bool>, IClientRequest
{
    public Guid PlanId { get; init; } = planId;

    public Guid ItemId { get; init; } = itemId;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class DeletePlanItemCommandHandler(IPlanItemRepository planItemRepository)
    : IRequestHandler<DeletePlanItemCommand, bool>
{
    public async Task<bool> Handle(DeletePlanItemCommand request, CancellationToken cancellationToken)
    {
        var planItem = await planItemRepository.GetByPlanAndItemIdAsync(request.PlanId, request.ItemId)
            ?? throw new NotFoundException($"Item is not found with this id: {request.ItemId}");

        var result = await planItemRepository.DeleteAsync(planItem);

        return result;
    }
}