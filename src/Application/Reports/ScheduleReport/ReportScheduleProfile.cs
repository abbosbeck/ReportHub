using Domain.Entities;

namespace Application.Reports.ScheduleReport;

public class ReportScheduleProfile : Profile
{
    public ReportScheduleProfile()
    {
        CreateMap<ScheduleReportRequest, ReportSchedule>()
            .ForMember(
                reportSchedule => reportSchedule.JobKey,
                configuration => configuration.MapFrom(request => $"Job-{Guid.NewGuid()}"))
            .ForMember(
                reportSchedule => reportSchedule.TriggerKey,
                configuration => configuration.MapFrom(request => $"Trigger-{Guid.NewGuid()}"));

        CreateMap<ReportSchedule, ReportScheduleDto>();
    }
}