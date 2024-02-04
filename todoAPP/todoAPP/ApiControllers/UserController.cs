using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using todoAPP.RequestModel;
using todoAPP.Services;
using Microsoft.AspNetCore.StaticFiles;

namespace todoAPP.ApiControllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _user;
        private readonly AvatarService _avartar;

        public UserController(UserService user, AvatarService avartar)
        {
            _user = user;
            _avartar = avartar;
        }

        [HttpGet]
        [Authorize]
        [Route("Info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var model = new GeneralRequestModel
            {
                Uid = _user.GetUserId()
            };
            return Ok(await _user.GetUserInfo(model));
        }

        [HttpPatch]
        [Authorize]
        [Route("Username")]
        public async Task<IActionResult> PatchUsername(PatchUsernameRequestModel model)
        {
            var patchUsernameModel = new PatchUsernameModel
            {
                UserId = _user.GetUserId(),
                Username = model.Username
            };

            await _user.UpdateUsername(patchUsernameModel);

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("Nickname")]
        public async Task<IActionResult> PatchUserNickname(PatchNicknameRequestModel model)
        {
            var patchNicknameModel = new PatchNicknameModel
            {
                UserId = _user.GetUserId(),
                Nickname = model.Nickname
            };

            await _user.UpdateNickname(patchNicknameModel);
            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("Password")]
        public async Task<IActionResult> PatchUserPassword(PatchPasswordRequestModel model)
        {
            var patchPasswordModel = new PatchPasswordModel
            {
                UserId = _user.GetUserId(),
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword,
                ConfirmNewPassword = model.NewPassword,
            };

            await _user.ChangePassword(patchPasswordModel);

            return Ok();
        }


        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("Role")]
        public async Task<IActionResult> PatchUserRole(PatchRoleRequestModel model)
        {
            await _user.UpdateUserRole(model);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("Avatar")]
        public async Task<IActionResult> GetUserAvatar()
        {
            string avatar = await _user.GetAvatar();

            if (string.IsNullOrEmpty(avatar))
            {
                avatar = "default.jpeg";
            }

            var stream = _avartar.ReadFile(avatar);
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(avatar, out string? contentType) == false)
            {
                contentType = "application/octet-stream";
            }

            return File(stream, contentType);
        }

        [HttpPatch]
        [Authorize]
        [Route("Avatar")]
        async public Task<IActionResult> PatchUserAvatar([FromForm] IFormFile avatarFile)
        {
            if (avatarFile.ContentType.Contains("image") == false)
            {
                return BadRequest("不支援的圖片格式");
            }

            var model = new PatchAvatarModel
            {
                UserId = _user.GetUserId(),
                File = avatarFile,
            };

            await _user.UpdateAvatar(model);

            return Ok();
        }
    }
}

