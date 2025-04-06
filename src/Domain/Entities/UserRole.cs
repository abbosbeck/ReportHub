using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; init; }

    public Role Role { get; init; }
}
