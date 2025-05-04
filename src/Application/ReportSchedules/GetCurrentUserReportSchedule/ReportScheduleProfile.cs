using Domain.Entities;

namespace Application.ReportSchedules.GetCurrentUserReportSchedule;

public class ReportScheduleProfile : Profile
{
    public ReportScheduleProfile()
    {
        CreateMap<ReportSchedule, ReportScheduleDto>();
    }
}