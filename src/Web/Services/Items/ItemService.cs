using Web.Models.Items;
using Web.Services.Customers;
using Web.Services.Invoices;

namespace Web.Services.Items;

public class ItemService(
    IHttpClientFactory httpClientFactory,
    IInvoiceService invoiceService)
    : IItemService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<List<ItemResponse>> GetItemListAsync(Guid clientId)
    {
        var response = await _httpClient.GetAsync($"clients/{clientId}/items");
        if (response.IsSuccessStatusCode)
        {
            var items = await response.Content.ReadFromJsonAsync<List<ItemResponse>>();

            var tasks = items.Select(async i =>
            {
                var invoice = await invoiceService.GetByIdAsync(i.InvoiceId, clientId);
                i.InvoiceNumber = invoice.InvoiceNumber.ToString("D6");
            });

            await Task.WhenAll(tasks);

            return items;
        }

        return new List<ItemResponse>();
    }
}
