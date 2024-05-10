using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
  private readonly UserService _user;
  private readonly IKanbanService _kanban;

  public AccountController(UserService user, IKanbanService kanban)
  {
    _user = user;
    _kanban = kanban;
  }

  [Route("/Register")]
  public async Task<IActionResult> Register(RegisterRequestModel model)
  {
    await _user.CreateUser(model);
    return Ok();
  }
}
