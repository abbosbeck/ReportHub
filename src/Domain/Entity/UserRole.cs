using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity
{
    public class UserRole : IdentityRole<Guid>, ISoftDeletable
    {
        public const string Admin = "Admin";

        public const string User = "User";

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
