
using Web.Models.Invoices;
using Web.Services.Customers;

namespace Web.Services.Invoices;

public class InvoiceService(
    IHttpClientFactory httpClientFactory,
    ICustomerService customerService)
    : IInvoiceService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<bool> CreateAsync(InvoiceCreateRequest invoice, Guid clienId)
    {
        var respone = await _httpClient.PostAsJsonAsync($"clients/{clienId}/invoices", invoice);
        if (respone.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid clientId)
    {
        var response = await _httpClient.DeleteAsync($"clients/{clientId}/invoices/{id}");
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<List<InvoiceResponse>> GetAllAsync(Guid clientId)
    {
        var invoiceResponse = await _httpClient.GetAsync($"clients/{clientId}/invoices");
        if (invoiceResponse.IsSuccessStatusCode)
        {
            var invoice = await invoiceResponse.Content.ReadFromJsonAsync<List<InvoiceResponse>>();
            var tasks = invoice.Select(async x =>
            {
                var customer = await customerService.GetByIdAsync(x.CustomerId, clientId);
                x.CustomerName = customer.Name;
            });

            await Task.WhenAll(tasks);

            return invoice;
        }

        return new List<InvoiceResponse>();
    }

    public async Task<InvoiceResponse> GetByIdAsync(Guid id, Guid clientId)
    {
        var invoiceResponse = await _httpClient.GetAsync($"clients/{clientId}/invoices/{id}");
        if (invoiceResponse.IsSuccessStatusCode)
        {
            var invoice = await invoiceResponse.Content.ReadFromJsonAsync<InvoiceResponse>();
            var customer = await customerService.GetByIdAsync(invoice.CustomerId, clientId);
            invoice.CustomerName = customer.Name;
            
            return invoice;
        }

        return new InvoiceResponse();
    }
}
