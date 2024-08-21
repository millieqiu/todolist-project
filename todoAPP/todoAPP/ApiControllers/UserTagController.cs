using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models.DTO;
using todoAPP.Models.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserTagController : ControllerBase
{
    private readonly IUserService _user;
    private readonly IUserTagService _userTag;
    private readonly Guid _userId;
    public UserTagController(IUserService user, IUserTagService userTag)
    {
        _user = user;
        _userTag = userTag;
        _userId = _user.GetUserId();
    }

    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetUserTagList()
    {
        return Ok(await _userTag.GetUserTagList(_userId));
    }

    [HttpPatch]
    public async Task<IActionResult> PatchUserTagName(IEnumerable<PatchUserTagNameRequestModel> model)
    {
        await _userTag.PatchUserTagName(new PatchUserTagDTO
        {
            UserId = _userId,
            TagList = model.Select(x => new PatchUserTagNameDTO
            {
                Uid = x.Uid,
                Name = x.Name,
            })
        });
        return Ok();
    }
}
