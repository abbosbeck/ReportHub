using Domain.Entities;

namespace Application.Plans.AddPlanItem;

public class PlanItemProfile : Profile
{
    public PlanItemProfile()
    {
        CreateMap<PlanItem, PlanItemDto>();
    }
}