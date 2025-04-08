using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class UserSystemRole : IdentityUserRole<Guid>, IBaseEntity, ISoftDeletable, IAuditableEntity
{
    public User User { get; init; }

    public SystemRole Role { get; init; }

    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string LastModifiedBy { get; set; }
}
