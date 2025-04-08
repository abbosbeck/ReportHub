using Domain.Common;

namespace Domain.Entities;

public class UserClientRole : BaseAuditableEntity, ISoftDeletable
{
    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; }

    public Guid ClientRoleId { get; set; }

    public ClientRole ClientRole { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
