namespace Web.Models.Reports;

public class ReportScheduleRequest
{
    public string JobKey { get; init; }

    public string TriggerKey { get; init; }

    public string CronExpression { get; set; }

    public Guid UserId { get; set; }
    
    public Guid ClientId { get; set; }
}