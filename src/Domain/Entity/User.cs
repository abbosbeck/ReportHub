using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity;

public class User : IdentityUser<Guid>, ISoftDeletable
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Department { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
