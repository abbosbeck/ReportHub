using Domain.Entities;

namespace Application.Plans.GetPlanById;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<Plan, PlanDto>()
            .ForMember(p => p.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Item, ItemDto>();
    }
}
