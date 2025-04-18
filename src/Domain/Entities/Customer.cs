using Domain.Common;

namespace Domain.Entities;

public class Customer : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string CountryCode { get; init; }

    public Guid ClientId { get; set; }

    public Client Client { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
