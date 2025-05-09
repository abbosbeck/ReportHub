using Web.Models;
using Web.Models.Users;

namespace Web.Services.Users
{
    public class UserService(IHttpClientFactory httpClientFactory) : IUserService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("api");

        public async Task<RegisterResponse> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("users/register", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponse>();
            }

            return new RegisterResponse();
        }
    }
}
