using Microsoft.AspNetCore.Mvc;
using todoAPP.Models.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class OAuthController : ControllerBase
{
    private readonly IOAuthService _oauth;
    private readonly IUserService _user;

    public OAuthController(IOAuthService oauth, IUserService user)
    {
        _oauth = oauth;
        _user = user;
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

        if (await _user.CheckUsernameDuplicated(userInfo.email) == false)
        {
            await _user.CreateUser(new RegisterRequestModel
            {
                Username = userInfo.email,
                Password = "",
                Nickname = userInfo.name,
            });
        }

        var user = await _user.QueryUser(userInfo.email);

        await _user.SignInUser(HttpContext, user);

        return new RedirectToPageResult("/Index");
    }
}

