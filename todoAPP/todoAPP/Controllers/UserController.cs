using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using todoAPP.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using todoAPP.RequestModel;
using todoAPP.Services;
using Microsoft.AspNetCore.StaticFiles;

namespace todoAPP.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly AuthService _auth;
        private readonly UserService _user;
        private readonly RoleService _role;

        public UserController(AuthService auth, UserService user, RoleService role)
        {
            _auth = auth;
            _user = user;
            _role = role;
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

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Role(PatchRoleRequestModel form)
        {
            User? user = _user.HasUser(form.UserID);

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "UserRole",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            _role.EditUserRole(user, form.RoleID);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Avatar()
        {
            User? user = _user.HasUser(_user.GetUserId());

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Avatar",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            Stream? stream = _user.GetAvatar(user.Avatar);
            if (stream == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Avatar",
                    Status = 2,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(user.Avatar, out string contentType) == false)
            {
                return File(stream, "application/octet-stream");
            }

            return File(stream, contentType);
        }

        [HttpPatch]
        [Authorize]
        async public Task<IActionResult> Avatar([FromForm] IFormFile avatar)
        {
            if (avatar.ContentType.Contains("image/") == false)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Avatar",
                    Status = 1,
                    ErrMsg = "Invalid input",
                };
                return BadRequest(err);
            }

            User? user = _user.HasUser(_user.GetUserId());

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "Avatar",
                    Status = 2,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            await _user.EditAvatar(user, avatar);

            return Ok();
        }
    }
}

