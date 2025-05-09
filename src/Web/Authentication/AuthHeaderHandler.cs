using Web.Services.Users;

namespace Web.Authentication;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IUserProviderService _tokenProvider;

    public AuthHeaderHandler(IUserProviderService tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _tokenProvider.GetToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
