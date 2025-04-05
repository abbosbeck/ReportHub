using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
