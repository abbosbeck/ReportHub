using Web.Models;

namespace Web.Services
{
    public class RegisterService(IHttpClientFactory httpClientFactory) : IRegisterService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

        public async Task<RegisterResponse> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("users/register", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponse>()
                       ?? throw new ApplicationException("Invalid response from server.");
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Registration failed: {error}");
        }
    }
}
