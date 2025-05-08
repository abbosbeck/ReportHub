using Web.Services;

namespace Web.Authentication;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ITokenProviderService _tokenProvider;

    public AuthHeaderHandler(ITokenProviderService tokenProvider)
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
