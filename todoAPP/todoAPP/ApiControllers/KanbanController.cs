using Microsoft.AspNetCore.Mvc;
using todoAPP.Models.DTO;
using todoAPP.Models.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
public class KanbanController : ControllerBase
{
	private readonly IUserService _user;
	private readonly IKanbanService _kanban;
	public KanbanController(IUserService user, IKanbanService kanban)
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
	[Route("{kanbanId}/Swimlane")]
	public async Task<IActionResult> CreateKanbanSwimlane(Guid kanbanId, CreateKanbanSwimlaneRequestModel model)
	{
		await _kanban.CreateKanbanSwimlane(new CreateKanbanSwimlaneDTO
		{
			KanbanId = kanbanId,
			Name = model.Name,
		});
		return Ok();
	}

	[HttpPatch]
	[Route("Swimlane/{swimlaneId}/Name")]
	public async Task<IActionResult> PatchKanbanSwimlaneName(Guid swimlaneId, PatchKanbanSwimlaneNameRequestModel model)
	{
		await _kanban.PatchKanbanSwimlaneName(new PatchKanbanSwimlaneNameDTO
		{
			KanbanSwimlaneId = swimlaneId,
			Name = model.Name,
		});
		return Ok();
	}

	[HttpPatch]
	[Route("Swimlane/Order")]
	public async Task<IActionResult> PatchKanbanSwimlaneName(PatchKanbanSwimlaneOrderRequestModel model)
	{
		await _kanban.PatchKanbanSwimlaneOrder(new PatchKanbanSwimlaneOrderDTO
		{
			DragSwimlaneId = model.DragSwimlaneId,
			DropPrevSwimlaneId = model.DropPrevSwimlaneId,
			DropNextSwimlaneId = model.DropNextSwimlaneId,
		});
		return Ok();
	}

	[HttpDelete]
	[Route("Swimlane/{swimlaneId}")]
	public async Task<IActionResult> DeleteKanbanSwimlane(Guid swimlaneId)
	{
		await _kanban.DeleteKanbanSwimlane(swimlaneId);
		return Ok();
	}
}
