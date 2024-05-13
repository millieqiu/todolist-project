using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("/api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserService _user;
    private readonly ITodoListService _todo;
    public AdminController(IUserService user, ITodoListService todo)
    {
        _user = user;
        _todo = todo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("User/{Uid}/Todo/List")]
    public async Task<IActionResult> AdminGetUserTodoList([FromRoute] GeneralRouteRequestModel route)
    {
        return Ok(await _todo.GetGeneralTodoList(new GetTodoListDTO
        {
            UserId = route.Uid
        }));
    }


    [HttpPatch]
    [Authorize(Roles = "Admin")]
    [Route("User/{Uid}/Role")]
    public async Task<IActionResult> PatchUserRole([FromRoute] GeneralRouteRequestModel route, PatchRoleRequestModel model)
    {
        await _user.UpdateUserRole(new PatchUserRoleDTO
        {
            UserId = route.Uid,
            Role = model.Role,
        });
        return Ok();
    }

    [HttpDelete]
    [Route("User/{Uid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] GeneralRouteRequestModel route)
    {
        await _user.DeleteUser(new DeleteUserDTO
        {
            UserId = route.Uid,
        });
        return Ok();
    }
}
