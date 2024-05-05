using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.DTO;
using todoAPP.Helpers;
using todoAPP.RequestModel;
using todoAPP.Services;
using todoAPP.ViewModel;

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
            return Ok(await _todo.GetSortedUserTodoList(new GetTodoListDTO
            {
                UserId = model.UserId
            }));
        }

        [HttpGet]
        [Route("Todo/List")]
        public async Task<IActionResult> GetTodoList([FromQuery] PaginationRequestModel model)
        {
            var todolist = await _todo.GetSortedUserTodoList(new GetTodoListDTO
            {
                UserId = _user.GetUserId(),
            });

            var first = todolist.Where(x => x.PrevId == Guid.Empty).SingleOrDefault() ?? throw new Exception("排序錯誤");
            var dict = new Dictionary<Guid, TodoViewModel>();
            foreach (var todo in todolist)
            {
                dict.Add(todo.Uid, todo);
            }
            return Ok(TraversalHelper.Traversal<TodoViewModel>(first, dict));
        }

        [HttpPost]
        [Route("Todo")]
        async public Task<IActionResult> CreateTodoItem(CreateTodoRequestModel model)
        {
            var todoItemId = await _todo.CreateTodoItem(new CreateTodoDTO
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
            await _todo.ChangeTodoItemStatus(model);

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

        [HttpPatch]
        [Route("UserTodo/Order")]
        public async Task<IActionResult> PatchUserTodoOrder(PatchUserTodoOrderRequestModel model)
        {
            await _todo.UpdateUserTodoOrder(new PatchUserTodoOrderDTO
            {
                Action = model.Action,
                DragTodoId = model.DragTodoId,
                DropTodoId = model.DropTodoId,
            });
            return Ok();
        }

        [HttpPatch]
        [Route("SwimlaneTodo/Order")]
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
            await _todo.DeleteUserAlreadyDoneTodoItem(new DeleteUserAlreadyDoneTodoDTO
            {
                UserId = _user.GetUserId(),
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

