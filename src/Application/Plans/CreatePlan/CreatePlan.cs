using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Plans.CreatePlan;

public class CreatePlanCommand(Guid clientId, CreatePlanRequest request) : IRequest<Guid>, IClientRequest
{
    public CreatePlanRequest Plan { get; set; } = request;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class CreatePlanCommandHandler(
    IMapper mapper,
    IItemRepository itemRepository,
    IPlanRepository planRepository,
    IClientRepository clientRepository,
    IValidator<CreatePlanRequest> validator)
    : IRequestHandler<CreatePlanCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Plan, cancellationToken: cancellationToken);

        _ = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var plan = mapper.Map<Plan>(request.Plan);
        var itemIds = request.Plan.PlanItems.Select(pi => pi.ItemId).ToList();

        if (itemIds.Count > 0)
        {
            var items = (await itemRepository.GetByIdsAsync(itemIds)).ToList();

            var missingIds = itemIds
                .FindAll(itemId => items.All(item => item.Id != itemId));

            if (missingIds.Count > 0)
            {
                throw new NotFoundException($"Items with these ids were not found: {string.Join(", ", missingIds)}");
            }

            plan.Items = request.Plan.PlanItems
                .Select(planItem => new PlanItem
                {
                    ItemId = planItem.ItemId,
                    PlanId = plan.Id,
                    Quantity = planItem.Quantity,
                })
                .ToList();
        }

        plan.ClientId = request.ClientId;

        await planRepository.AddAsync(plan);

        return plan.Id;
    }
}
