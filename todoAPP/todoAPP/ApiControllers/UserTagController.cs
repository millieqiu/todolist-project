using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserTagController : ControllerBase
{
    private readonly UserService _user;
    private readonly IUserTagService _userTag;
    public UserTagController(UserService user, IUserTagService userTag)
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
}
