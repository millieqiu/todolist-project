﻿using Microsoft.AspNetCore.Mvc;
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
            if (gtvm == null)
            {
                throw new Exception("GoogleTokenViewModel object is null");
            }

            GoogleUserinfoViewModel? userInfo = await _oauth.GetUserInfo(gtvm.access_token);
            if (userInfo == null)
            {
                throw new Exception("GoogleUserinfoViewModel object is null");
            }

            User? user = _user.HasUser(userInfo.email);
            if (user == null)
            {
                _user.CreateUser(userInfo.email, "", userInfo.name);
                user = _user.HasUser(userInfo.email);
                if (user == null)
                {
                    throw new Exception("User object is null");
                }
            }
            await _user.SignInUser(HttpContext, user);

            return new RedirectToPageResult("/TodoPage");
        }
    }
}

