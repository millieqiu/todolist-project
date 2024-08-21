using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models.DTO;
using todoAPP.Models.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("/api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserService _user;
    private readonly ITodoService _todo;
    public AdminController(IUserService user, ITodoService todo)
    {
        _user = user;
        _todo = todo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("User/{userId}/Todo/List")]
    public async Task<IActionResult> AdminGetUserTodoList(Guid userId)
    {
        return Ok(await _todo.GetGeneralTodoList(userId));
    }


    [HttpPatch]
    [Authorize(Roles = "Admin")]
    [Route("User/{userId}/Role")]
    public async Task<IActionResult> PatchUserRole(Guid userId, PatchRoleRequestModel model)
    {
        await _user.UpdateUserRole(new PatchUserRoleDTO
        {
            UserId = userId,
            Role = model.Role,
        });
        return Ok();
    }

    [HttpDelete]
    [Route("User/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _user.DeleteUser(userId);
        return Ok();
    }
}
