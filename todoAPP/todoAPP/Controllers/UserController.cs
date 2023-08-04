using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using todoAPP.ViewModel;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace todoAPP.Pages.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly DataContext _db;


        public UserController(DataContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginForm)
        {
            User? user = _db.Users
                .Where(x => x.Username == loginForm.Username)
                .SingleOrDefault();
            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    service = "Login",
                    status = 1,
                    errMsg = "User dosen't exists.",
                };
                return BadRequest(err);
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            string derivedPassword = PasswordGenerator(loginForm.Password, salt);

            bool equal = KeyDerivation.Equals(user.Password, derivedPassword);
            if(equal == false)
            {
                return BadRequest("Validation error.");
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid,user.ID.ToString()), //使用者ID
                new Claim(ClaimTypes.NameIdentifier,user.Username),  //使用者名稱
                new Claim(ClaimTypes.Name,user.Nickname)  //使用者名稱
            };

            var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = userPrincipal;
            var props = new AuthenticationProperties();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,userPrincipal, props);

            UserInfoViewModel respObj = new UserInfoViewModel
            {
                ID = user.ID,
                Username = user.Username,
                Nickname = user.Nickname
            };
            return Ok(respObj);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string Sid = claims.First().Value;
                string username = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                string nickname = claims.First(x => x.Type == ClaimTypes.Name).Value;
                Int32.TryParse(Sid, out int userId);

                UserInfoViewModel info = new UserInfoViewModel()
                {
                    ID = userId,
                    Username = username,
                    Nickname = nickname,
                };
                return Ok(info);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("Logout success");
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterReqViewModel registerForm)
        {
            List<User> list = _db.Users
                .Where(x => x.Username == registerForm.Username)
                .ToList();
            if (list.Count > 0)
            {
                return BadRequest("Username exist");
            }

            byte[] salt = CreateSalt();
            string derivedPassword = PasswordGenerator(registerForm.Password, salt);

            User user = new User
            {
                Username = registerForm.Username,
                Password = derivedPassword,
                Nickname = registerForm.Nickname,
                Salt = Convert.ToBase64String(salt)
            };

            var result = _db.Users.Add(user);
            _db.SaveChanges();
            UserInfoViewModel respObj = new UserInfoViewModel
            {
                ID = result.Entity.ID,
                Username = result.Entity.Username,
                Nickname = result.Entity.Nickname
            };
            return Ok(respObj);
        }

        private static string PasswordGenerator(string password, byte[] salt)
        {
            byte[] key = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,//鹽
                prf: KeyDerivationPrf.HMACSHA512,//偽隨機函數
                iterationCount: 1000,//雜湊執行次數
                numBytesRequested: 256 / 8
                );
            return Convert.ToBase64String(key);
        }

        private static byte[] CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}

