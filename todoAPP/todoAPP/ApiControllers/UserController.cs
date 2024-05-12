using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using todoAPP.RequestModel;
using todoAPP.Services;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using todoAPP.DTO;

namespace todoAPP.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _user;
    private readonly AvatarService _avartar;

    public UserController(IUserService user, AvatarService avartar)
    {
        _user = user;
        _avartar = avartar;
    }

    [HttpGet]
    [Authorize]
    [Route("Info")]
    public async Task<IActionResult> GetUserInfo()
    {
        return Ok(await _user.GetUserInfo(new GeneralRouteRequestModel
        {
            Uid = _user.GetUserId()
        }));
    }

    [HttpPatch]
    [Authorize]
    [Route("Username")]
    public async Task<IActionResult> PatchUsername(PatchUsernameRequestModel model)
    {
        try
        {
            await _user.UpdateUsername(new PatchUsernameDTO
            {
                UserId = _user.GetUserId(),
                Username = model.Username
            });
        }
        catch (DuplicateNameException)
        {
            return BadRequest("帳號已存在");
        }

        return Ok();
    }

    [HttpPatch]
    [Authorize]
    [Route("Nickname")]
    public async Task<IActionResult> PatchUserNickname(PatchNicknameRequestModel model)
    {
        await _user.UpdateNickname(new PatchNicknameDTO
        {
            UserId = _user.GetUserId(),
            Nickname = model.Nickname
        });
        return Ok();
    }

    [HttpPatch]
    [Authorize]
    [Route("Password")]
    public async Task<IActionResult> PatchUserPassword(PatchPasswordRequestModel model)
    {
        await _user.ChangePassword(new PatchPasswordDTO
        {
            UserId = _user.GetUserId(),
            OldPassword = model.OldPassword,
            NewPassword = model.NewPassword,
        });

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
    public async Task<IActionResult> PatchUserAvatar([FromForm] IFormFile avatarFile)
    {
        if (avatarFile.ContentType.Contains("image") == false)
        {
            return BadRequest("不支援的圖片格式");
        }

        await _user.UpdateAvatar(new PatchAvatarDTO
        {
            UserId = _user.GetUserId(),
            File = avatarFile,
        });

        return Ok();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        await _user.DeleteUser(new DeleteUserDTO
        {
            UserId = _user.GetUserId(),
        });
        return Ok();
    }

}

