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
    [Route("{Uid}")]
    public async Task<IActionResult> PatchUserTagName([FromRoute] GeneralRouteRequestModel route, PatchUserTagNameRequestModel model)
    {
        await _userTag.PatchUserTagName(new PatchUserTagNameDTO
        {
            UserTagId = route.Uid,
            Name = model.Name,
        });
        return Ok();
    }
}
