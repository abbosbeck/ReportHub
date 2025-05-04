using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ReportScheduleRepository(AppDbContext context) : IReportScheduleRepository
{
    public async Task<ReportSchedule> AddAsync(ReportSchedule reportSchedule)
    {
        await context.AddAsync(reportSchedule);
        await context.SaveChangesAsync();

        return reportSchedule;
    }

    public async Task<ReportSchedule> UpdateAsync(ReportSchedule reportSchedule)
    {
        context.Update(reportSchedule);
        await context.SaveChangesAsync();

        return reportSchedule;
    }

    public async Task<ReportSchedule> GetByUserIdAsync(Guid userId)
    {
        return await context.ReportSchedules.FirstOrDefaultAsync(reportSchedule => reportSchedule.UserId == userId);
    }

    public async Task<bool> DeleteAsync(ReportSchedule reportSchedule)
    {
        context.Remove(reportSchedule);

        return await context.SaveChangesAsync() > 0;
    }
}