using System.Collections.ObjectModel;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Quartz;

namespace Infrastructure.Jobs;

public class ReportScheduleService(ISchedulerFactory schedulerFactory) : IReportScheduleService
{
    private readonly Task<IScheduler> schedulerTask = schedulerFactory.GetScheduler();

    public async Task ScheduleAsync(ReportSchedule reportSchedule)
    {
        var scheduler = await schedulerTask;

        var job = JobBuilder.Create<ReportEmailSenderJob>()
            .WithIdentity(reportSchedule.JobKey)
            .UsingJobData(nameof(reportSchedule.UserId), reportSchedule.UserId)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(reportSchedule.TriggerKey)
            .WithCronSchedule(reportSchedule.CronExpression)
            .ForJob(reportSchedule.JobKey)
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task ReScheduleAsync(ReportSchedule reportSchedule)
    {
        var scheduler = await schedulerTask;

        await scheduler.RescheduleJob(
            new TriggerKey(reportSchedule.TriggerKey),
            TriggerBuilder.Create()
                .WithIdentity(reportSchedule.TriggerKey)
                .WithCronSchedule(reportSchedule.CronExpression)
                .ForJob(reportSchedule.JobKey)
                .Build());
    }

    public async Task<bool> DeleteAsync(ReportSchedule reportSchedule)
    {
        var scheduler = await schedulerTask;

        return await scheduler.DeleteJob(new JobKey(reportSchedule.JobKey));
    }

    public async Task RestoreScheduledJobsAsync(List<ReportSchedule> reportSchedules)
    {
        var scheduler = await schedulerTask;

        var jobsAndTriggers = reportSchedules.ToDictionary(
            reportSchedule => JobBuilder.Create<ReportEmailSenderJob>()
                .WithIdentity(reportSchedule.JobKey)
                .UsingJobData(nameof(reportSchedule.UserId), reportSchedule.UserId)
                .Build(),
            IReadOnlyCollection<ITrigger> (reportSchedule) => new List<ITrigger>()
            {
                TriggerBuilder.Create()
                    .WithIdentity(reportSchedule.TriggerKey)
                    .WithCronSchedule(reportSchedule.CronExpression)
                    .ForJob(reportSchedule.JobKey)
                    .Build(),
            });

        await scheduler.ScheduleJobs(jobsAndTriggers, true);
    }
}