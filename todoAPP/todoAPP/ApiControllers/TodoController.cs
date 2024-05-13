using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoListService _todo;
    private readonly IUserService _user;

    public TodoController(ITodoListService todo, IUserService user)
    {
        _todo = todo;
        _user = user;
    }

    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetTodoList([FromQuery] PaginationRequestModel model)
    {
        return Ok(await _todo.GetGeneralTodoList(new GetTodoListDTO
        {
            UserId = _user.GetUserId(),
        }));
    }

    [HttpPost]
    async public Task<IActionResult> CreateTodoItem(CreateTodoRequestModel model)
    {
        return Ok(await _todo.CreateTodo(new CreateTodoDTO
        {
            Title = model.Title,
            Description = model.Description,
            ExecuteAt = model.ExecuteAt ?? DateTimeOffset.UnixEpoch,
            UserId = _user.GetUserId(),
        }));
    }

    [HttpPatch]
    [Route("{Uid}/Status")]
    public async Task<IActionResult> ChangeTodoStatus([FromRoute] GeneralRouteRequestModel model)
    {
        await _todo.UpdateTodoStatus(model);
        return Ok();
    }

    [HttpPatch]
    [Route("{Uid}/Info")]
    public async Task<IActionResult> UpdateTodoInfo([FromRoute] GeneralRouteRequestModel route, PatchTodoInfoRequestModel model)
    {
        await _todo.UpdateTodoInfo(new PatchTodoInfoDTO
        {
            Title = model.Title,
            Description = model.Description,
            ExecuteAt = model.ExecuteAt ?? DateTimeOffset.UnixEpoch,
            TodoId = route.Uid,
        });
        return Ok();
    }

    [HttpPatch]
    [Route("{Uid}/Tag")]
    public async Task<IActionResult> UpdateTodoTag([FromRoute] GeneralRouteRequestModel route, PatchTodoTagRequestModel model)
    {
        await _todo.UpdateTodoTag(new PatchTodoTagDTO
        {
            TodoId = route.Uid,
            TagId = model.TagId,
        });
        return Ok();
    }

    [HttpPatch]
    [Route("Order/General")]
    public async Task<IActionResult> PatchGeneralTodoOrder(PatchGeneralTodoOrderRequestModel model)
    {
        await _todo.UpdateGeneralTodoOrder(new PatchGeneralTodoOrderDTO
        {
            UserId = _user.GetUserId(),
            DragTodoId = model.DragTodoId,
            DropPrevTodoId = model.DropPrevTodoId,
            DropNextTodoId = model.DropNextTodoId,
        });
        return Ok();
    }

    [HttpPatch]
    [Route("Order/Swimlane")]
    public async Task<IActionResult> PatchSwimlaneTodoOrder(PatchSwimlaneTodoOrderRequestModel model)
    {
        await _todo.UpdateSwimlaneTodoOrder(new PatchSwimlaneTodoOrderDTO
        {
            TodoId = model.TodoId,
            KanbanSwimlaneId = model.KanbanSwimlaneId,
            DropPrevTodoId = model.DropPrevTodoId,
            DropNextTodoId = model.DropNextTodoId,
        });
        return Ok();
    }

    [HttpDelete]
    [Route("Done")]
    public async Task<IActionResult> DeleteAlreadyDoneTodoItem()
    {
        await _todo.DeleteAlreadyDoneTodo(new DeleteAlreadyDoneTodoDTO
        {
            UserId = _user.GetUserId(),
        });
        return Ok();
    }

    [HttpDelete]
    [Route("{Uid}")]
    public async Task<IActionResult> Delete([FromRoute] GeneralRouteRequestModel model)
    {
        await _todo.DeleteTodo(model);
        return Ok();
    }
}

