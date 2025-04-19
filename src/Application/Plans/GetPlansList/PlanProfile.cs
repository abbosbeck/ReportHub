using Domain.Entities;

namespace Application.Plans.GetPlansList;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<Plan, PlanDto>();
    }
}
