using Domain.Entities;

namespace Application.ExportReports.ExportReportsToFile;

public class ExportReportsToFileProfile : Profile
{
    public ExportReportsToFileProfile()
    {
        CreateMap<Plan, PlanDto>();
    }
}
