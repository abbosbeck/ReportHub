using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class SystemRole : IdentityRole<Guid>, ISoftDeletable, IBaseEntity, IAuditableEntity
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string LastModifiedBy { get; set; }
}