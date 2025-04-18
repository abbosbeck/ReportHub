namespace Application.Common.Interfaces.Authorization;

public interface IClientIdProvider
{
    public Guid ClientId { get; set; }
}
