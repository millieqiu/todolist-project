using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public interface IOAuthService
    {
        public string GetProviderURL();

        public Task<OAuthTokenViewModel?> GetToken(string code);

        public Task<OAuthUserinfoViewModel?> GetUserInfo(string accessToken);
    }
    
    public class OAuthService : IOAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public OAuthService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public string GetProviderURL()
        {
            Uri authUri = new Uri("https://accounts.google.com/o/oauth2/v2/auth");
            QueryBuilder qBuilder = new QueryBuilder
            {
                { "client_id", _configuration.GetSection("Authentication:Google:ClientId").Value },
                { "response_type", "code" },
                { "scope", "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email" },
                { "redirect_uri", "https://localhost:44334/api/OAuth/Callback" }
            };

            string queryParamStr = qBuilder.ToQueryString().ToUriComponent();
            Uri queryUri = new Uri(authUri, queryParamStr);

            return queryUri.ToString();
        }

        async public Task<OAuthTokenViewModel?> GetToken(string code)
        {
            string tokenUri = "https://oauth2.googleapis.com/token";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "client_id", _configuration.GetSection("Authentication:Google:ClientId").Value },
                { "client_secret", _configuration.GetSection("Authentication:Google:ClientSecret").Value },
                { "redirect_uri", "https://localhost:44334/api/OAuth/Callback" }
            };

            HttpClient httpClient = _clientFactory.CreateClient();
            var response = await httpClient.PostAsync(tokenUri, new FormUrlEncodedContent(param));
            var contents = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OAuthTokenViewModel>(contents);
        }

        async public Task<OAuthUserinfoViewModel?> GetUserInfo(string accessToken)
        {
            Uri userInfoUri = new Uri("https://www.googleapis.com/oauth2/v1/userinfo");
            QueryBuilder qBuilder = new QueryBuilder
            {
                { "access_token", accessToken }
            };
            string queryParamStr = qBuilder.ToQueryString().ToUriComponent();
            Uri queryUri = new Uri(userInfoUri, queryParamStr);

            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetStringAsync(queryUri);
            return JsonSerializer.Deserialize<OAuthUserinfoViewModel>(response);
        }
    }
}

