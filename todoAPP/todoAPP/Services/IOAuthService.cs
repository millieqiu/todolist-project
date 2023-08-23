using todoAPP.ViewModel;

namespace todoAPP.Services
{
	public interface IOAuthService
    {
        public string GetProviderURL();

        public Task<GoogleTokenViewModel?> GetToken(string code);

        public Task<GoogleUserinfoViewModel?> GetUserInfo(string accessToken);
    }
}

