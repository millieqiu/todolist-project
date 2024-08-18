using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserTagController : ControllerBase
{
    private readonly IUserService _user;
    private readonly IUserTagService _userTag;
    public UserTagController(IUserService user, IUserTagService userTag)
    {
        _user = user;
        _userTag = userTag;
    }

    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetUserTagList()
    {
        return Ok(await _userTag.GetUserTagList(new GetUserTagListDTO
        {
            UserId = _user.GetUserId(),
        }));
    }

    [HttpPatch]
    public async Task<IActionResult> PatchUserTagName(IEnumerable<PatchUserTagNameRequestModel> model)
    {
        await _userTag.PatchUserTagName(new PatchUserTagDTO
        {
            UserId = _user.GetUserId(),
            TagList = model.Select(x => new PatchUserTagNameDTO
            {
                Uid = x.Uid,
                Name = x.Name,
            })
        });
        return Ok();
    }
}
