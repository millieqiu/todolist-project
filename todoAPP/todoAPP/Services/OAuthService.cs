using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using todoAPP.ConfigModels;
using todoAPP.ViewModel;

namespace todoAPP.Services;

public interface IOAuthService
{
    public string GetProviderEndpoint();

    public Task<OAuthTokenViewModel> GetToken(string code);

    public Task<OAuthUserinfoViewModel> GetUserInfo(string accessToken);
}

public class OAuthService : IOAuthService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IHttpClientFactory _clientFactory;
    private readonly OAuthConfig _oauthConfig;
    private Uri _requestUrl;

    public OAuthService(IHttpContextAccessor accessor, IHttpClientFactory clientFactory, OAuthConfig oauthConfig)
    {
        _accessor = accessor;
        _clientFactory = clientFactory;
        _oauthConfig = oauthConfig;
        _requestUrl = new Uri($"{_accessor.HttpContext!.Request.Scheme}://{_accessor.HttpContext!.Request.Host}");
    }

    public string GetProviderEndpoint()
    {
        var authUri = new Uri("https://accounts.google.com/o/oauth2/v2/auth");
        var qBuilder = new QueryBuilder
            {
                { "client_id", _oauthConfig.Google.ClientId },
                { "response_type", "code" },
                { "scope", "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email" },
                { "redirect_uri", new Uri(_requestUrl,"/api/OAuth/Login").ToString() }
            };

        string queryParamStr = qBuilder.ToQueryString().ToUriComponent();

        return new Uri(authUri, queryParamStr).AbsoluteUri;
    }

    async public Task<OAuthTokenViewModel> GetToken(string code)
    {
        var param = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "client_id", _oauthConfig.Google.ClientId },
                { "client_secret", _oauthConfig.Google.ClientSecret },
                { "redirect_uri", new Uri(_requestUrl,"/api/OAuth/Login").ToString() }
            };

        var httpClient = _clientFactory.CreateClient();
        var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(param));
        var contents = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OAuthTokenViewModel>(contents)
            ?? throw new Exception("GetToken fail");
    }

    async public Task<OAuthUserinfoViewModel> GetUserInfo(string accessToken)
    {
        var userInfoUri = new Uri("https://www.googleapis.com/oauth2/v1/userinfo");
        var qBuilder = new QueryBuilder
            {
                { "access_token", accessToken }
            };
        var queryParamStr = qBuilder.ToQueryString().ToUriComponent();
        var queryUri = new Uri(userInfoUri, queryParamStr);

        var httpClient = _clientFactory.CreateClient();
        var response = await httpClient.GetStringAsync(queryUri);
        return JsonSerializer.Deserialize<OAuthUserinfoViewModel>(response)
            ?? throw new Exception("GetUserInfo fail");
    }
}

