using System.Net.Http.Headers;
using Web.Models.Customers;

namespace Web.Services.Customers;

public class CustomerService(IHttpClientFactory httpClientFactory) : ICustomerService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<bool> CreateAsync(CreateCustomerRequest customer, Guid clientId)
    {
        var respone = await _httpClient.PostAsJsonAsync($"clients/{clientId}/customers", customer);
        if (respone.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid clientId)
    {
        var response = await _httpClient.DeleteAsync($"clients/{clientId}/customers/{id}");
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<CustomerResponse> GetByIdAsync(Guid id, Guid clientId)
    {
        var response = await _httpClient.GetAsync($"clients/{clientId}/customers/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CustomerResponse>();
        }

        return new CustomerResponse();
    }

    public async Task<List<CustomerResponse>> GetListAsync(Guid clientId)
    {
        var response = await _httpClient.GetAsync($"clients/{clientId}/customers");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
        }

        return new List<CustomerResponse>();
    }

    public async Task<List<CustomerResponse>> UploadCustomerListAsync(byte[] file, string name, Guid clientId)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(file);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
        content.Add(fileContent, "file", "customers.xlsx");

        var response = await _httpClient.PostAsync($"clients/{clientId}/customers/import", content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
        }

        return new List<CustomerResponse>();
    }
}
