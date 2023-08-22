using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using todoAPP.Services;
using todoAPP.ViewModel;

namespace todoAPP.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class OAuthController : Controller
    {
        private readonly OAuthService _oauth;
        private readonly UserService _user;

        public OAuthController(OAuthService oauth, UserService user)
        {
            _oauth = oauth;
            _user = user;
        }

        [HttpGet]
        public IActionResult RedirectToServiceProvider()
        {
            return Ok(_oauth.GetProviderURL());
        }

        [HttpGet]
        async public Task<IActionResult> Callback(string code)
        {

            GoogleTokenViewModel? gtvm = await _oauth.GetToken(code);
            if(gtvm == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            GoogleUserinfoViewModel? userInfo = await _oauth.GetUserInfo(gtvm.access_token);
            if (userInfo == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            User? user = _user.HasUser(userInfo.email);
            if(user == null)
            {
                _user.CreateUser(userInfo.email, "", userInfo.name);
                user = _user.HasUser(userInfo.email);
                if(user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            await _user.SignInUser(HttpContext,user);

            return new RedirectToPageResult("/TodoPage");
        }
    }
}

