namespace Web.Models.Reports;

public class ReportScheduleResponse
{
    public Guid Id { get; init; }
    
    public string JobKey { get; init; }

    public string TriggerKey { get; init; }

    public string CronExpression { get; set; }

    public Guid UserId { get; set; }
    
    public Guid ClientId { get; set; }
}