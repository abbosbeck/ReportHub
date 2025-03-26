using Domain.Common;

namespace Domain;

public class User : BaseAuditableEntity, ISoftDeletable
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
