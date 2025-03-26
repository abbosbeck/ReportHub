using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities;
public class User : BaseAuditableEntity, ISoftDeletable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsDeleted { get; set; }
}
