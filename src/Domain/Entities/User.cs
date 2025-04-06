using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>, ISoftDeletable, IBaseEntity, IAuditableEntity
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Department { get; init; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string LastModifiedBy { get; set; }
}
