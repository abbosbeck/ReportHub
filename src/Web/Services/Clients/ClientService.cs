using Web.Models.Clients;

namespace Web.Services.Clients;

public class ClientService(IHttpClientFactory httpClientFactory) : IClientService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<List<ClientResponse>> GetListAsync()
    {
        var response = await _httpClient.GetAsync("clients");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<List<ClientResponse>>();
    }
}