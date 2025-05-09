using Web.Models.Clients;

namespace Web.Services.Clients;

public interface IClientService
{
    Task<List<ClientResponse>> GetListAsync();
}