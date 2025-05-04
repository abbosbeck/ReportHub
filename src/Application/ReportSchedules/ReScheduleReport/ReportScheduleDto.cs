namespace Application.ReportSchedules.ReScheduleReport;

public class ReportScheduleDto
{
    public Guid Id { get; init; }

    public string JobKey { get; init; }

    public string TriggerKey { get; init; }

    public string CronExpression { get; init; }

    public Guid UserId { get; init; }

    public Guid ClientId { get; init; }
}