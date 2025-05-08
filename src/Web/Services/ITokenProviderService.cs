namespace Web.Services;

public interface ITokenProviderService
{
    void SetToken(string token);

    string GetToken();

    string GetUserEmail();
}
