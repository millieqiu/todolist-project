using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public class OAuthService
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
                { "client_id", _configuration["Authentication:Google:ClientId"] },
                { "response_type", "code" },
                { "scope", "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email" },
                { "redirect_uri", "https://localhost:7236/api/OAuth/Callback" }
            };
            string queryParamStr = qBuilder.ToQueryString().ToUriComponent();
            Uri queryUri = new Uri(authUri, queryParamStr);
            return queryUri.ToString();
        }

        async public Task<GoogleTokenViewModel?> GetToken(string code)
        {
            string oauthUri = "https://oauth2.googleapis.com/token";
            var param = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "client_id", _configuration["Authentication:Google:ClientId"] },
                { "client_secret", _configuration["Authentication:Google:ClientSecret"] },
                { "redirect_uri", "https://localhost:7236/api/OAuth/Callback" }
            };

            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.PostAsync(oauthUri, new FormUrlEncodedContent(param));
            var contents = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GoogleTokenViewModel>(contents);
        }

        async public Task<GoogleUserinfoViewModel?> GetUserInfo(string accessToken)
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
            return JsonSerializer.Deserialize<GoogleUserinfoViewModel>(response);
        }
    }
}

