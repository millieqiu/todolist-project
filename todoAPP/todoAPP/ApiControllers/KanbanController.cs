using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
public class KanbanController : ControllerBase
{
  private readonly UserService _user;
  private readonly IKanbanService _kanban;
  public KanbanController(UserService user, IKanbanService kanban)
  {
    _user = user;
    _kanban = kanban;
  }

  [HttpGet]
  [Route("List")]
  public async Task<IActionResult> GetKanbanList()
  {
    return Ok(await _kanban.GetKanbanList(new GetKanbanListDTO
    {
      UserId = _user.GetUserId(),
    }));
  }

  [HttpGet]
  [Route("Swimlane/List")]
  public async Task<IActionResult> GetKanbanSwimlaneList()
  {
    var kanbanId = await _kanban.GetDefaultKanbanId(new GetDefaultKanbanDTO
    {
      UserId = _user.GetUserId(),
    });
    var swimlaneList = await _kanban.GetKanbanSwimlaneList(new GetKanbanSwimlaneListDTO
    {
      KanbanId = kanbanId,
    });
    return Ok(swimlaneList);
  }

  [HttpPost]
  [Route("{Uid}/Swimlane")]
  public async Task<IActionResult> CreateKanbanSwimlane([FromRoute] GeneralRequestModel route, CreateKanbanSwimlaneRequestModel model)
  {
    await _kanban.CreateKanbanSwimlane(new CreateKanbanSwimlaneDTO
    {
      KanbanId = route.Uid,
      Name = model.Name,
    });
    return Ok();
  }

  [HttpPatch]
  [Route("Swimlane/{Uid}/Name")]
  public async Task<IActionResult> PatchKanbanSwimlaneName([FromRoute] GeneralRequestModel route, PatchKanbanSwimlaneNameRequestModel model)
  {
    await _kanban.PatchKanbanSwimlaneName(new PatchKanbanSwimlaneNameDTO
    {
      KanbanSwimlaneId = route.Uid,
      Name = model.Name,
    });
    return Ok();
  }

  [HttpPatch]
  [Route("Swimlane/{Uid}/Order")]
  public async Task<IActionResult> PatchKanbanSwimlaneName([FromRoute] GeneralRequestModel route, PatchKanbanSwimlaneOrderRequestModel model)
  {
    await _kanban.PatchKanbanSwimlaneOrder(new PatchKanbanSwimlaneOrderDTO
    {
      DragSwimlaneId = route.Uid,
      DropPrevSwimlaneId = model.DropPrevSwimlaneId,
      DropNextSwimlaneId = model.DropNextSwimlaneId,
    });
    return Ok();
  }

  [HttpDelete]
  [Route("Swimlane/{Uid}")]
  public async Task<IActionResult> DeleteKanbanSwimlane([FromRoute] GeneralRequestModel route)
  {
    await _kanban.DeleteKanbanSwimlane(new DeleteKanbanSwimlaneDTO
    {
      KanbanSwimlaneId = route.Uid,
    });
    return Ok();
  }
}
