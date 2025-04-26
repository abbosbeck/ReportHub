using Domain.Entities;

namespace Application.Plans.GetPlanById;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<Plan, PlanDto>();

        CreateMap<Item, ItemDto>();

        CreateMap<PlanItem, PlanItemDto>();
    }
}
