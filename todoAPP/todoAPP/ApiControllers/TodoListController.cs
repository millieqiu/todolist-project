using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("/api")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todo;
        private readonly UserService _user;

        public TodoListController(ITodoListService todo, UserService user, WeatherService weather)
        {
            _todo = todo;
            _user = user;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("User/{UserId}/Todo/List")]
        public async Task<IActionResult> AdminGetUserTodoList([FromRoute] GetTodoListRequestModel model)
        {
            return Ok(await _todo.GetTodoList(new GetTodoListDTO
            {
                Page = model.Page,
                Limit = model.Limit,
                UserId = model.UserId
            }));
        }

        [HttpGet]
        [Route("Todo/List")]
        public async Task<IActionResult> GetTodoList([FromQuery] PaginationRequestModel model)
        {
            return Ok(await _todo.GetTodoList(new GetTodoListDTO
            {
                Page = model.Page,
                Limit = model.Limit,
                UserId = new Guid(HttpContext.User.FindFirstValue(ClaimTypes.Sid))
            }));
        }

        [HttpPost]
        [Route("Todo")]
        async public Task<IActionResult> CreateTodoItem(CreateTodoRequestModel model)
        {
            var todoItemId = await _todo.CreateTodoItem(new CreateTodoDTO
            {
                Title = model.Title,
                Description = model.Description,
                UserId = _user.GetUserId(),
            });

            return Ok(todoItemId);
        }

        [HttpPatch]
        [Route("Todo/{Uid}/Status")]
        public async Task<IActionResult> ChangeTodoStatus([FromRoute] GeneralRequestModel model)
        {
            await _todo.ChangeTodoItemStatus(model);

            return Ok();
        }

        [HttpPatch]
        [Route("Todo/{Uid}/Swimlane")]
        public async Task<IActionResult> ChangeTodoSwimlane([FromRoute] Guid Uid, PatchTodoSwimlaneRequestModel model)
        {
            await _todo.ChangeTodoSwimlane(new PatchTodoSwimlaneDTO
            {
                TodoId = Uid,
                KanbanSwimlaneId = model.KanbanSwimlaneId,
            });
            return Ok();
        }

        [HttpDelete]
        [Route("Todo/{Uid}")]
        public async Task<IActionResult> Delete([FromRoute] GeneralRequestModel model)
        {
            await _todo.DeleteTodoItem(model);

            return Ok();
        }
    }
}

