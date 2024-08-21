using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using todoAPP.Services;
using Microsoft.AspNetCore.StaticFiles;
using todoAPP.Models.RequestModel;
using todoAPP.Models.DTO;

namespace todoAPP.ApiControllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _user;
    private readonly AvatarService _avartar;
    private readonly Guid _userId;

    public UserController(IUserService user, AvatarService avartar)
    {
        _user = user;
        _avartar = avartar;
        _userId = _user.GetUserId();
    }

    [HttpGet]
    [Route("Info")]
    public async Task<IActionResult> GetUserInfo()
    {
        return Ok(await _user.GetUserInfo(_userId));
    }

    [HttpPatch]
    [Route("Username")]
    public async Task<IActionResult> PatchUsername(PatchUsernameRequestModel model)
    {
        await _user.UpdateUsername(new PatchUsernameDTO
        {
            UserId = _userId,
            Username = model.Username
        });

        return Ok();
    }

    [HttpPatch]
    [Route("Nickname")]
    public async Task<IActionResult> PatchUserNickname(PatchNicknameRequestModel model)
    {
        await _user.UpdateNickname(new PatchNicknameDTO
        {
            UserId = _userId,
            Nickname = model.Nickname
        });
        return Ok();
    }

    [HttpPatch]
    [Route("Password")]
    public async Task<IActionResult> PatchUserPassword(PatchPasswordRequestModel model)
    {
        await _user.ChangePassword(new PatchPasswordDTO
        {
            UserId = _userId,
            OldPassword = model.OldPassword,
            NewPassword = model.NewPassword,
        });

        return Ok();
    }

    [HttpGet]
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
    [Route("Avatar")]
    public async Task<IActionResult> PatchUserAvatar([FromForm] IFormFile avatarFile)
    {
        if (avatarFile.ContentType.Contains("image") == false)
        {
            return BadRequest("不支援的圖片格式");
        }

        await _user.UpdateAvatar(new PatchAvatarDTO
        {
            UserId = _userId,
            File = avatarFile,
        });

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        await _user.DeleteUser(_userId);
        return Ok();
    }
}

