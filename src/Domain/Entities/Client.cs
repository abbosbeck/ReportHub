using Domain.Common;

namespace Domain.Entities;

public class Client : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
