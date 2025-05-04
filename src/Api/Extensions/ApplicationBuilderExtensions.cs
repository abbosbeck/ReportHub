using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task RestoreScheduledJobsAsync(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        var service = services.GetRequiredService<IReportScheduleService>();
        var reportSchedules = await context.ReportSchedules.ToListAsync();

        await service.RestoreScheduledJobsAsync(reportSchedules);
    }
}