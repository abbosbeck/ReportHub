namespace Web.Services;

public interface ITokenProviderService
{
    void SetToken(string token);

    void RemoveToken();

    string GetToken();

    string GetUserEmail();
}
