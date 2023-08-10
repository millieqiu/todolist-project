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

namespace todoAPP.Controllers
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
            User? user = HasUser(loginForm.Username);

            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            string derivedPassword = PasswordGenerator(
                loginForm.Password,
                Convert.FromBase64String(user.Salt)
            );

            bool equal = KeyDerivation.Equals(user.Password, derivedPassword);
            if (equal == false)
            {
                return BadRequest("Invalid username or password");
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid,user.ID.ToString()), //使用者ID
                new Claim(ClaimTypes.NameIdentifier,user.Username),  //使用者名稱
                new Claim(ClaimTypes.Name,user.Nickname)  //使用者名稱
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = userPrincipal;
            var props = new AuthenticationProperties();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, props);

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
            User? user = HasUser(registerForm.Username);

            if (user != null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Register",
                    Status = 1,
                    ErrMsg = "Username is already exists",
                };
                return BadRequest(err);
            }

            CreateUser(registerForm.Username, registerForm.Password, registerForm.Nickname);

            return Ok();
        }

        [HttpPatch]
        public IActionResult Role(PatchRoleRequestModel form)
        {
            if(CheckUserRole(GetUserId(), "admin") == false)
            {
                return Unauthorized();
            }

            User? user = GetUser(form.UserID);
            Role? role = HasRole(form.RoleName);

            if (user == null || role == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "UserRole",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            EditUserRole(user, role);

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

        private User? GetUser(int id)
        {
            return _db.Users
                .Where(x => x.ID == id)
                .SingleOrDefault();
        }

        private User? HasUser(string username)
        {
            return _db.Users
                .Where(x => x.Username == username)
                .SingleOrDefault();
        }

        private void CreateUser(string username, string password, string nickname)
        {
            byte[] salt = CreateSalt();

            User user = new User
            {
                Username = username,
                Password = PasswordGenerator(password, salt),
                Nickname = nickname,
                Salt = Convert.ToBase64String(salt),
                Role = HasRole("user")
            };

            _db.Users.Add(user);
            _db.SaveChanges();
        }

        private Role? HasRole(string roleName)
        {

            return _db.Roles
                .Where(x => x.Name == roleName)
                .SingleOrDefault();
        }

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string Sid = claims.First().Value;
                Int32.TryParse(Sid, out int userId);
                return userId;
            }
            return 0;
        }

        private bool CheckUserRole(int userID, string roleName)
        {
            return _db.Users.Where(x => x.Role.Name == roleName && x.ID == userID).Any();
        }

        private void EditUserRole(User user, Role role)
        {
            user.Role = role;
            _db.SaveChanges();
        }
    }
}

