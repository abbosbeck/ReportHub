using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Application.Plans.DeletePlan;

public class DeletePlanCommand(Guid planId, Guid clientId) : IRequest<bool>, IClientRequest
{
    public Guid Id { get; set; } = planId;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner)]
public class DeletePlanCommandHandler(IPlanRepository repository)
    : IRequestHandler<DeletePlanCommand, bool>
{
    public async Task<bool> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Plan is not found with this id: {request.ClientId}");

        var isSucceed = await repository.RemoveAsync(plan);

        return isSucceed;
    }
}
