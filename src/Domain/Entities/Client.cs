using Domain.Common;

namespace Domain.Entities;

public class Client : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
