using Domain.Common;

namespace Domain.Entities;

public class ClientRoleAssignment : BaseAuditableEntity, ISoftDeletable
{
    public Guid UserId { get; init; }

    public User User { get; init; }

    public Guid ClientId { get; init; }

    public Client Client { get; init; }

    public Guid ClientRoleId { get; init; }

    public ClientRole ClientRole { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
