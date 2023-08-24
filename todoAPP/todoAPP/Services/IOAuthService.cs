using todoAPP.ViewModel;

namespace todoAPP.Services
{
	public interface IOAuthService
    {
        public string GetProviderURL();

        public Task<OAuthTokenViewModel?> GetToken(string code);

        public Task<OAuthUserinfoViewModel?> GetUserInfo(string accessToken);
    }
}

