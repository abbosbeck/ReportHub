using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity
{
    public class Role : IdentityRole<Guid>, ISoftDeletable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}