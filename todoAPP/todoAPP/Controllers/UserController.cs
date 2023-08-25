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

        public UserController(UserService user, RoleService role)
        {
            _user = user;
            _role = role;
        }

        [HttpPatch]
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

