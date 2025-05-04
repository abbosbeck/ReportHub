using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IReportScheduleService
{
    Task ScheduleAsync(ReportSchedule reportSchedule);

    Task ReScheduleAsync(ReportSchedule reportSchedule);

    Task<bool> DeleteAsync(ReportSchedule reportSchedule);

    Task RestoreScheduledJobsAsync(List<ReportSchedule> reportSchedules);
}