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
using todoAPP.RequestModel;

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
        public async Task<IActionResult> Login(LoginRequestModel loginForm)
        {
            User? user = _db.Users
                .Where(x => x.Username == loginForm.Username)
                .SingleOrDefault();
            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Login",
                    Status = 1,
                    ErrMsg = "User dosen't exists.",
                };
                return BadRequest(err);
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            string derivedPassword = PasswordGenerator(loginForm.Password, salt);

            bool equal = KeyDerivation.Equals(user.Password, derivedPassword);
            if(equal == false)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Login",
                    Status = 2,
                    ErrMsg = "Validation error.",
                };
                return BadRequest(err);
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

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        public IActionResult Register(RegisterRequestModel registerForm)
        {
            bool hasAccount = _db.Users
                .Where(x => x.Username == registerForm.Username)
                .Any();
            if (hasAccount == true)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Register",
                    Status = 1,
                    ErrMsg = "Username is already exists",
                };
                return BadRequest(err);
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

            return Ok();
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

