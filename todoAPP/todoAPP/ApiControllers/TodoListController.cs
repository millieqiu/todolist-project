using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AdminGetUserTodoList([FromRoute] GetTodoListModel model)
        {
            return Ok(await _todo.GetTodoList(model));
        }

        [HttpGet]
        [Route("Todo/List")]
        public async Task<IActionResult> GetTodoList([FromQuery]PaginationRequestModel model)
        {
            var reqModel = new GetTodoListModel
            {
                Page = model.Page,
                Limit = model.Limit,
                UserId = new Guid(HttpContext.User.FindFirstValue(ClaimTypes.Sid))
            };

            return Ok(await _todo.GetTodoList(reqModel));
        }

        [HttpPost]
        [Route("Todo")]
        async public Task<IActionResult> CreateTodoItem(CreateTodoRequestModel model)
        {
            var createTodoModel = new CreateTodoModel
            {
                Text = model.Text,
                UserId = _user.GetUserId(),
            };

            var todoItemId = await _todo.CreateTodoItem(createTodoModel);

            return Ok(todoItemId);
        }

        [HttpPatch]
        [Route("Todo/{Uid}/Status")]
        public async Task<IActionResult> ChangeTodoStatus([FromRoute] GeneralRequestModel model)
        {
            await _todo.ChangeTodoItemStatus(model);

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

