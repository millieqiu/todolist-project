using System.Security.Claims;
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
    [Route("/api/[controller]/[action]")]
    public class TodoListController : Controller
    {
        private readonly TodoListService _todo;
        private readonly UserService _user;
        private readonly RoleService _role;


        public TodoListController(TodoListService todo, UserService user, RoleService role)
        {
            _todo = todo;
            _user = user;
            _role = role;
        }

        [HttpGet]
        public IActionResult ListUserPagination(int page, int userId)
        {

            if (_role.CheckUserRole(GetUserId(), "admin") != true)
            {
                return Unauthorized();
                //
            }

            if (page < 1)
            {
                page = 1;
            }

            PaginationViewModel response = new PaginationViewModel()
            {
                NumOfPages = _todo.GetNumOfPages(GetUserId()),
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
                NumOfPages = _todo.GetNumOfPages(GetUserId()),
                CurrentPage = page,
                List = _todo.GetPaginatedData(page, GetUserId()),
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(CreateTodoRequestModel request)
        {
            User? user = _user.HasUser(GetUserId());
            if (user == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "CreateTodoItem",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound();
            }

            int id = _todo.CreateItem(request.Text, user);

            return Ok(new GeneralViewModel() { ID = id });
        }

        [HttpPut]
        public IActionResult ChangeStatus(GeneralRequestModel request)
        {
            Todo? item = _todo.HasItem(GetUserId(),request.ID);

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
            Todo? item = _todo.HasItem(GetUserId(),request.ID);

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

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string Sid = claims.First().Value;
                Int32.TryParse(Sid, out int userId);
                return userId;
            }
            return 0;
        }
    }
}

