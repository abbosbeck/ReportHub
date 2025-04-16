using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Plans.CreatePlan;
public class CreatePlanCommand : IRequest<Guid>
{
    public string Title { get; set; }

    public ICollection<Guid> ItemIds { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid OwnerId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class CreatePlanCommandHandler(
    IItemRepository itemRepository,
    IPlanRepository planRepository,
    IClientRepository clientRepository,
    IValidator<CreatePlanCommand> validator)
    : IRequestHandler<CreatePlanCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.OwnerId);

        if (client == null)
        {
            throw new NotFoundException($"Client is not found with this id: {request.OwnerId}");
        }

        var newPlan = new Plan()
        {
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Items = new List<Item>(),
        };

        if (request.ItemIds.Count > 0)
        {
            var items = await itemRepository.GetByIdsAsync(request.ItemIds);

            if (items == null)
            {
                throw new NotFoundException($"Items with these ids were not found: {string.Join(", ", request.ItemIds)}");
            }

            var missingIds = new List<Guid>();

            foreach (var requestedId in request.ItemIds)
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

            foreach (var item in items)
            {
                newPlan.Items.Add(item);
            }
        }

        var plan = await planRepository.AddAsync(newPlan);

        return plan.Id;
    }
}
