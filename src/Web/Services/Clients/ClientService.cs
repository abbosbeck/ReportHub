using Web.Models.Clients;

namespace Web.Services.Clients;

public class ClientService(IHttpClientFactory httpClientFactory) : IClientService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<bool> CreateAsync(ClientCreateRequest client)
    {
        var respone = await _httpClient.PostAsJsonAsync("clients", client);
        if (respone.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<List<ClientResponse>> GetListAsync()
    {
        var response = await _httpClient.GetAsync("clients");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<ClientResponse>>();
        }
        
        return new List<ClientResponse>();
    }

    public async Task<List<ClientResponse>> GetUserClients()
    {
        var response = await _httpClient.GetAsync("clients/my");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<ClientResponse>>();
        }

        return new List<ClientResponse>();
    }
}