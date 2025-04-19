using Application.Common.Interfaces.Authorization;

namespace Application.Common.Configurations;

public class ClientIdProvider : IClientIdProvider
{
    public Guid ClientId { get; set; }
}
