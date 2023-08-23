using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.Services;
using todoAPP.ViewModel;

namespace todoAPP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class TodoListController : Controller
    {
        private readonly TodoListService _todo;
        private readonly UserService _user;


        public TodoListController(TodoListService todo, UserService user, WeatherService weather)
        {
            _todo = todo;
            _user = user;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/api/admin/[controller]")]
        public IActionResult ListUserPagination(int page, int userId)
        {

            if (page < 1)
            {
                page = 1;
            }

            PaginationViewModel response = new PaginationViewModel()
            {
                NumOfPages = _todo.GetNumOfPages(_user.GetUserId()),
                CurrentPage = page,
                List = _todo.GetPaginatedData(page, userId),
            };

            return Ok(response);
        }

        [HttpGet]
        public IActionResult ListPagination(int page)
        {
            if (page < 1)
            {
                page = 1;
            }

            PaginationViewModel response = new PaginationViewModel()
            {
                NumOfPages = _todo.GetNumOfPages(_user.GetUserId()),
                CurrentPage = page,
                List = _todo.GetPaginatedData(page, _user.GetUserId()),
            };

            return Ok(response);
        }

        [HttpPost]
        async public Task<IActionResult> Create(CreateTodoRequestModel request)
        {
            User? user = _user.HasUser(_user.GetUserId());
            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "CreateTodoItem",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            int id = await _todo.CreateItem(request.Text, user);

            return Ok(new GeneralViewModel() { ID = id });
        }

        [HttpPatch]
        [Route("Status")]
        public IActionResult ChangeStatus(GeneralRequestModel request)
        {
            Todo? item = _todo.HasItem(_user.GetUserId(), request.ID);

            if (item == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "EditTodoItem",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            _todo.ChangeItemStatus(item);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(GeneralRequestModel request)
        {
            Todo? item = _todo.HasItem(_user.GetUserId(), request.ID);

            if (item == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "DeleteTodoItem",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            _todo.DeleteItem(item);

            return Ok();
        }
    }
}

