using Web.Models;

namespace Web.Services
{
    public interface IRegisterService
    {
        Task<RegisterResponse> RegisterAsync(RegisterModel model);
    }
}
