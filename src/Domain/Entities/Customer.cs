using Domain.Common;

namespace Domain.Entities;

public class Customer : BaseAuditableEntity, ISoftDeletable
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string Country { get; init; }

    public Guid ClientId { get; init; }

    public Client Client { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
