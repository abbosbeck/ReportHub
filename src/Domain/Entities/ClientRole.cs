using Domain.Common;

namespace Domain.Entities;

public class ClientRole : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
