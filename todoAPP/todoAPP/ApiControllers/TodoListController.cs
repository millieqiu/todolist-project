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

        public TodoListController(ITodoListService todo, UserService user)
        {
            _todo = todo;
            _user = user;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("User/{UserId}/Todo/List")]
        public async Task<IActionResult> AdminGetUserTodoList([FromRoute] GetTodoListRequestModel model)
        {
            return Ok(await _todo.GetGeneralTodoList(new GetTodoListDTO
            {
                UserId = model.UserId
            }));
        }

        [HttpGet]
        [Route("Todo/List")]
        public async Task<IActionResult> GetTodoList([FromQuery] PaginationRequestModel model)
        {
            var todolist = await _todo.GetGeneralTodoList(new GetTodoListDTO
            {
                UserId = _user.GetUserId(),
            });
            return Ok(todolist);
        }

        [HttpPost]
        [Route("Todo")]
        async public Task<IActionResult> CreateTodoItem(CreateTodoRequestModel model)
        {
            var todoItemId = await _todo.CreateTodo(new CreateTodoDTO
            {
                Title = model.Title,
                Description = model.Description,
                ExecuteAt = model.ExecuteAt ?? DateTimeOffset.UnixEpoch,
                UserId = _user.GetUserId(),
            });

            return Ok(todoItemId);
        }

        [HttpPatch]
        [Route("Todo/{Uid}/Status")]
        public async Task<IActionResult> ChangeTodoStatus([FromRoute] GeneralRequestModel model)
        {
            await _todo.UpdateTodoStatus(model);

            return Ok();
        }

        [HttpPatch]
        [Route("Todo/{Uid}/Info")]
        public async Task<IActionResult> UpdateTodoInfo([FromRoute] GeneralRequestModel route, PatchTodoInfoRequestModel model)
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
        [Route("Todo/{Uid}/Tag")]
        public async Task<IActionResult> UpdateTodoTag([FromRoute] GeneralRequestModel route, PatchTodoTagRequestModel model)
        {
            await _todo.UpdateTodoTag(new PatchTodoTagDTO
            {
                TodoId = route.Uid,
                TagId = model.TagId,
            });

            return Ok();
        }

        [HttpPatch]
        [Route("Todo/Order/General")]
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
        [Route("Todo/Order/Swimlane")]
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
        [Route("Todo/Done")]
        public async Task<IActionResult> DeleteAlreadyDoneTodoItem()
        {
            await _todo.DeleteAlreadyDoneTodo(new DeleteAlreadyDoneTodoDTO
            {
                UserId = _user.GetUserId(),
            });
            return Ok();
        }

        [HttpDelete]
        [Route("Todo/{Uid}")]
        public async Task<IActionResult> Delete([FromRoute] GeneralRequestModel model)
        {
            await _todo.DeleteTodo(model);

            return Ok();
        }
    }
}

