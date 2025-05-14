using Domain.Entities;

namespace Application.Plans.CreatePlan;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<CreatePlanRequest, Plan>();
        CreateMap<CreatePlanItemDto, PlanItem>();
    }
}
