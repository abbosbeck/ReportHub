using Domain.Entities;

namespace Application.ReportSchedules.ReScheduleReport;

public class ReportScheduleProfile : Profile
{
    public ReportScheduleProfile()
    {
        CreateMap<ReScheduleReportRequest, ReportSchedule>()
            .ForMember(
                reportSchedule => reportSchedule.JobKey,
                configuration => configuration.MapFrom(request => $"Job-{Guid.NewGuid()}"))
            .ForMember(
                reportSchedule => reportSchedule.TriggerKey,
                configuration => configuration.MapFrom(request => $"Trigger-{Guid.NewGuid()}"));

        CreateMap<ReportSchedule, ScheduleReport.ReportScheduleDto>();
    }
}