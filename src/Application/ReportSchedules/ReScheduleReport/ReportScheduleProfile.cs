using Domain.Entities;

namespace Application.ReportSchedules.ReScheduleReport;

public class ReportScheduleProfile : Profile
{
    public ReportScheduleProfile()
    {
        CreateMap<ReportSchedule, ReportScheduleDto>();
        CreateMap<ReScheduleReportRequest, ReportSchedule>();
    }
}