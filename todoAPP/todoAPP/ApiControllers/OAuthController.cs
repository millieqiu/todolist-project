using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class OAuthController : ControllerBase
    {
        private readonly IOAuthService _oauth;
        private readonly UserService _user;
        private readonly IKanbanService _kanban;

        public OAuthController(IOAuthService oauth, UserService user, IKanbanService kanban)
        {
            _oauth = oauth;
            _user = user;
            _kanban = kanban;
        }

        [HttpGet]
        public IActionResult ProviderEndpoint()
        {
            return Ok(_oauth.GetProviderEndpoint());
        }

        [HttpGet]
        async public Task<IActionResult> Login(string code)
        {
            var token = await _oauth.GetToken(code);

            var userInfo = await _oauth.GetUserInfo(token.access_token);

            var model = new RegisterRequestModel
            {
                Username = userInfo.email,
                Password = "",
                Nickname = userInfo.name,
            };

            if (await _user.CheckUsernameDuplicated(userInfo.email) == false)
            {
                await _user.CreateUser(model);
            }

            var user = await _user.QueryUser(userInfo.email);

            await _user.SignInUser(HttpContext, user);

            return new RedirectToPageResult("/Index");
        }
    }
}

