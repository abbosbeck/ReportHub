using Web.Models.Items;
using Web.Services.Customers;

namespace Web.Services.Items;

public class ItemService(
    IHttpClientFactory httpClientFactory,
    ICustomerService customerService)
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
                var customer = await customerService.GetByIdAsync(i.CustomerId, clientId);
                i.CustomerName = customer.Name;
            });

            await Task.WhenAll(tasks);

            return items;
        }

        return new List<ItemResponse>();
    }
}
