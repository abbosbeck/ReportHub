using Domain.Entities;

namespace Application.Reports.ReScheduleReport;

public class ReportScheduleProfile : Profile
{
    public ReportScheduleProfile()
    {
        CreateMap<ReportSchedule, ReportScheduleDto>();
        CreateMap<ReScheduleReportRequest, ReportSchedule>();
    }
}