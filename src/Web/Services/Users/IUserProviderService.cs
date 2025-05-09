using Web.Models.Users;

namespace Web.Services.Users;

public interface IUserProviderService
{
    void SetToken(string token);

    void RemoveToken();

    string GetToken();

    string GetUserEmail();

    UserRoles GetRoles();
}
