using Domain.Entities;

namespace Application.Plans.UpdatePlan;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<UpdatePlanRequest, Plan>();
        CreateMap<Plan, PlanDto>();
    }
}
