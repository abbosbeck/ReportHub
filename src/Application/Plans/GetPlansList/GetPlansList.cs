using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Plans.GetPlansList;

public class GetPlansListQuery(Guid clientId) : IRequest<List<PlanDto>>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class GetPlansListQueryHandler(
    IPlanRepository repository,
    IMapper mapper)
    : IRequestHandler<GetPlansListQuery, List<PlanDto>>
{
    public async Task<List<PlanDto>> Handle(GetPlansListQuery request, CancellationToken cancellationToken)
    {
        var plans = await repository.GetAll()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<PlanDto>>(plans);
    }
}
