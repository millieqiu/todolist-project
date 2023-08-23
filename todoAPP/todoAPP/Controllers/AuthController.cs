using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using todoAPP.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.Controllers
{
    [ApiController]
    [Route("/api/[action]")]
    public class AuthController : Controller
    {
        private readonly AuthService _auth;
        private readonly UserService _user;

        public AuthController(AuthService auth, UserService user)
        {
            _auth = auth;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel loginForm)
        {
            Thread.Sleep(2000);//顯示loading延遲
            User? user = _user.HasUser(loginForm.Username);

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Login",
                    Status = 1,
                    ErrMsg = "Invalid username or password",
                };
                return BadRequest(err);
            }

            string derivedPassword = _auth.PasswordGenerator(
                loginForm.Password,
                Convert.FromBase64String(user.Salt)
            );

            if (KeyDerivation.Equals(user.Password, derivedPassword) == false)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Login",
                    Status = 1,
                    ErrMsg = "Invalid username or password",
                };
                return BadRequest(err);
            }

            await _user.SignInUser(HttpContext, user);

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
            User? user = _user.HasUser(registerForm.Username);

            if (user != null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Register",
                    Status = 1,
                    ErrMsg = "Username already exists",
                };
                return BadRequest(err);
            }

            _user.CreateUser(registerForm.Username, registerForm.Password, registerForm.Nickname);

            return Ok();
        }
    }
}

