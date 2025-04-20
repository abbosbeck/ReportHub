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
    IItemRepository itemRepository,
    IPlanRepository planRepository,
    IClientRepository clientRepository,
    IPlanItemRepository planItemRepository,
    IMapper mapper,
    IValidator<CreatePlanRequest> validator)
    : IRequestHandler<CreatePlanCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Plan, cancellationToken: cancellationToken);

        _ = await clientRepository.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {request.ClientId}");

        var newPlan = mapper.Map<Plan>(request.Plan);

        if (request.Plan.PlanItemDtos.Count > 0)
        {
            var items = await itemRepository.GetByIdsAsync(request.Plan.PlanItemDtos.Select(pi => pi.ItemId))
                ?? throw new NotFoundException(
                    $"Items with these ids were not found: " +
                    $"{string.Join(", ", request.Plan.PlanItemDtos.Select(pi => pi.ItemId))}");

            var missingIds = new List<Guid>();
            foreach (var requestedId in request.Plan.PlanItemDtos.Select(pi => pi.ItemId))
            {
                bool exists = items.Any(item => item.Id == requestedId);
                if (!exists)
                {
                    missingIds.Add(requestedId);
                }
            }

            if (missingIds.Count > 0)
            {
                throw new NotFoundException($"Items with these ids were not found: {string.Join(", ", missingIds)}");
            }
            var newItems = new List<Item>();
            foreach (var item in items)
            {
                newItems.Add(item);
            }
            newPlan.Items = newItems;
            newPlan.ClientId = request.ClientId;
        }


        var plan = await planRepository.AddAsync(newPlan);

        var planItems = request.Plan.PlanItemDtos
            .Select(dto => new PlanItem
            {
                ItemId = dto.ItemId,
                PlanId = plan.Id,
                Quantity = dto.Quantity
            })
            .ToList();
        await planItemRepository.AddBulkAsync(planItems);

        return plan.Id;
    }
}
