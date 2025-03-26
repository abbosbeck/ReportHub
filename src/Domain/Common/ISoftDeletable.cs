namespace Domain.Common;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
