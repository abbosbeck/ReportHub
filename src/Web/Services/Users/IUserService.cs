using Web.Models;
using Web.Models.Users;

namespace Web.Services.Users
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterAsync(RegisterModel model);
    }
}
