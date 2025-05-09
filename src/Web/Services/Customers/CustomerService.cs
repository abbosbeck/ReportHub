namespace Web.Services.Customers;

public class CustomerService(IHttpClientFactory httpClientFactory) : ICustomerService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<List<CustomerResponse>> GetListAsync(Guid clientId)
    {
        var response = await _httpClient.GetAsync($"clients/{clientId}/customers");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
        }

        return new List<CustomerResponse>();
    }
}
