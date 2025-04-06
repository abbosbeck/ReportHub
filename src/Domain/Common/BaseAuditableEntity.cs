namespace Domain.Common;

public abstract class BaseAuditableEntity : IBaseEntity, IAuditableEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string LastModifiedBy { get; set; }
}