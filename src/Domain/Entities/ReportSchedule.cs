using Domain.Common;

namespace Domain.Entities;

public class ReportSchedule : BaseAuditableEntity, ISoftDeletable
{
    public string JobKey { get; init; }

    public string TriggerKey { get; init; }

    public string CronExpression { get; init; }

    public Guid UserId { get; set; }

    public User User { get; init; }

    public Guid ClientId { get; init; }

    public Client Client { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}