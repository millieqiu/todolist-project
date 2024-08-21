using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models.DTO;
using todoAPP.Models.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todo;
    private readonly IUserService _user;
    private readonly Guid _userId;

    public TodoController(ITodoService todo, IUserService user)
    {
        _todo = todo;
        _user = user;
        _userId = _user.GetUserId();
    }

    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetTodoList()
    {
        return Ok(await _todo.GetGeneralTodoList(_userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem(CreateTodoRequestModel model)
    {
        return Ok(await _todo.CreateTodo(new CreateTodoDTO
        {
            Title = model.Title,
            Description = model.Description,
            ExecuteAt = model.ExecuteAt,
            TagId = model.TagId,
            UserId = _userId,
        }));
    }

    [HttpPatch]
    [Route("{todoId}/Status")]
    public async Task<IActionResult> ChangeTodoStatus(Guid todoId)
    {
        await _todo.UpdateTodoStatus(todoId);
        return Ok();
    }

    [HttpPatch]
    [Route("{todoId}/Info")]
    public async Task<IActionResult> UpdateTodoInfo(Guid todoId, PatchTodoInfoRequestModel model)
    {
        await _todo.UpdateTodoInfo(new PatchTodoInfoDTO
        {
            Title = model.Title,
            Description = model.Description,
            ExecuteAt = model.ExecuteAt,
            TodoId = todoId,
        });
        return Ok();
    }

    [HttpPatch]
    [Route("{todoId}/Tag")]
    public async Task<IActionResult> UpdateTodoTag(Guid todoId, PatchTodoTagRequestModel model)
    {
        await _todo.UpdateTodoTag(new PatchTodoTagDTO
        {
            TodoId = todoId,
            TagId = model.TagId,
            UserId = _userId,
        });
        return Ok();
    }

    [HttpPatch]
    [Route("Order/General")]
    public async Task<IActionResult> PatchGeneralTodoOrder(PatchGeneralTodoOrderRequestModel model)
    {
        await _todo.UpdateGeneralTodoOrder(new PatchGeneralTodoOrderDTO
        {
            UserId = _userId,
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
            UserId = _userId,
        });
        return Ok();
    }

    [HttpDelete]
    [Route("{todoId}")]
    public async Task<IActionResult> Delete(Guid todoId)
    {
        await _todo.DeleteTodo(todoId);
        return Ok();
    }
}

