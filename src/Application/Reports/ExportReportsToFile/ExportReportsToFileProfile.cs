using Domain.Entities;

namespace Application.Reports.ExportReportsToFile;

public class ExportReportsToFileProfile : Profile
{
    public ExportReportsToFileProfile()
    {
        CreateMap<Plan, PlanDto>();
    }
}
