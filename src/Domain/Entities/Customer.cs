using Domain.Common;

namespace Domain.Entities;

public class Customer : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string CountryCode { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
