using Web.Models.Clients;

namespace Web.Services.Clients;

public interface IClientService
{
    Task<List<ClientResponse>> GetListAsync();

    Task<List<ClientResponse>> GetUserClients();

    Task<bool> CreateAsync(ClientCreateRequest client);
}