using Web.Models;

namespace Web.Services;

public interface IUserProviderService
{
    void SetToken(string token);

    void RemoveToken();

    string GetToken();

    string GetUserEmail();

    UserRoles GetRoles();
}
