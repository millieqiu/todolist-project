using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using todoAPP.ViewModel;
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
        private readonly UserService _user;
        private readonly RoleService _role;
        private readonly AuthService _auth;

        public UserController(UserService user, RoleService role, AuthService auth)
        {
            _user = user;
            _role = role;
            _auth = auth;
        }

        [HttpPatch]
        [Authorize]
        public IActionResult Username(PatchUsernameRequestModel form)
        {
            User? user = _user.HasUser(_user.GetUserId());

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "User",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            bool result = _user.EditUsername(user, form.Username);
            if (result == false)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "User",
                    Status = 2,
                    ErrMsg = "Invalid username",
                };
                return BadRequest(err);
            }

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        public IActionResult Nickname(EditNicknameRequestModel form)
        {
            User? user = _user.HasUser(_user.GetUserId());

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "User",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            _user.EditNickname(user, form.Nickname);
            return Ok();
        }

        [HttpPatch]
        [Authorize]
        public IActionResult Password(PatchPasswordRequestModel form)
        {
            User? user = _user.HasUser(_user.GetUserId());

            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "User",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }


            if (_auth.PasswordValidator(user.Password, user.Salt, form.OldPassword) == false)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "User",
                    Status = 2,
                    ErrMsg = "Invalid password",
                };
                return BadRequest(err);
            }

            _user.ChangePassword(user, form.NewPassword);

            return Ok();
        }


        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("/api/admin/[controller]/[action]")]
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

