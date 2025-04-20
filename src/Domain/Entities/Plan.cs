using Domain.Common;

namespace Domain.Entities;
public class Plan : BaseAuditableEntity, ISoftDeletable
{
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ICollection<Item> Items { get; set; }

    public decimal TotalPrice { get; set; }

    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }
}
