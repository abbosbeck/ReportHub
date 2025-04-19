using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Plans.UpdatePlan;

public class UpdatePlanCommand(Guid clientId, UpdatePlanRequest request) : IRequest<PlanDto>, IClientRequest
{
    public UpdatePlanRequest Plan { get; set; } = request;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class UpdatePlanCommandHandler(
    IPlanRepository repository,
    IValidator<UpdatePlanRequest> validator,
    IMapper mapper)
    : IRequestHandler<UpdatePlanCommand, PlanDto>
{
    public async Task<PlanDto> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Plan, cancellationToken);

        var plan = await repository.GetByIdAsync(request.Plan.Id)
            ?? throw new NotFoundException($"Plan is not found with this id: {request.ClientId}");
        
        mapper.Map(request.Plan, plan);

        var updatedPlan = await repository.UpdateAsync(plan);

        return mapper.Map<PlanDto>(updatedPlan);
    }
}
