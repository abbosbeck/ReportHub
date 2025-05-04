using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IReportScheduleRepository
{
    Task<ReportSchedule> AddAsync(ReportSchedule reportSchedule);

    Task<ReportSchedule> UpdateAsync(ReportSchedule reportSchedule);

    Task<ReportSchedule> GetByUserIdAsync(Guid userId);

    Task<bool> DeleteAsync(ReportSchedule reportSchedule);
}