using Web.Models.Items;
using Web.Services.ExternalServices;
using Web.Services.Invoices;

namespace Web.Services.Items;

public class ItemService(
    IHttpClientFactory httpClientFactory,
    IMoneyService moneyService,
    IInvoiceService invoiceService)
    : IItemService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

    public async Task<bool> CreateAsync(CreateItemRequest item, Guid clientId)
    {
        var respone = await _httpClient.PostAsJsonAsync($"clients/{clientId}/items", item);
        if (respone.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid clientId)
    {
        var response = await _httpClient.DeleteAsync($"clients/{clientId}/items/{id}");
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

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
                i.PriceDto = moneyService.GetAmountWithSymbol(i.Price, i.CurrencyCode);
            });

            await Task.WhenAll(tasks);

            return items;
        }

        return new List<ItemResponse>();
    }
}
