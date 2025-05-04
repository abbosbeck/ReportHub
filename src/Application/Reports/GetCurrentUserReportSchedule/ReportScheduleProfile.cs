using Domain.Entities;

namespace Application.Reports.GetCurrentUserReportSchedule;

public class ReportScheduleProfile : Profile
{
    public ReportScheduleProfile()
    {
        CreateMap<ReportSchedule, ReportScheduleDto>();
    }
}