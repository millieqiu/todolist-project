using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class TodoListController : Controller
    {
        private readonly DataContext _db;


        public TodoListController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult ListUserPagination(int page, int userId)
        {

            if (CheckUserRole(GetUserId(), "admin") != true)
            {
                return Unauthorized();
            }

            if (page < 1)
            {
                page = 1;
            }

            PaginationViewModel response = new PaginationViewModel()
            {
                NumOfPages = GetNumOfPages(),
                CurrentPage = page,
                List = GetPaginatedData(page, userId),
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
                NumOfPages = GetNumOfPages(),
                CurrentPage = page,
                List = GetPaginatedData(page, GetUserId()),
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(CreateTodoRequestModel request)
        {
            User? user = _db.Users.Find(GetUserId());
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

            int id = CreateItem(request.Text, user);

            return Ok(new GeneralViewModel() { ID = id });
        }

        [HttpPut]
        public IActionResult ChangeStatus(GeneralRequestModel request)
        {
            Todo? item = HasItem(request.ID);

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

            ChangeItemStatus(item);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(GeneralRequestModel request)
        {
            Todo? item = HasItem(request.ID);

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

            DeleteItem(item);

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

        private List<Todo> GetPaginatedData(int page, int userId)
        {
            return _db.TodoList
                .Where(x => x.User.ID == userId)
                .OrderByDescending(item => item.CreatedAt)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();
        }

        private double GetNumOfPages()
        {
            int count = _db.TodoList.Where(x => x.User.ID == GetUserId()).Count();
            return Math.Ceiling(count / 10.0);
        }

        public bool CheckUserRole(int userID, string roleName)
        {
            return _db.Users.Where(x => x.Role.Name == roleName && x.ID == userID).Any();
        }

        private Todo? HasItem(int todoItemId)
        {
            return _db.TodoList
                .Where(x => x.User.ID == GetUserId() && x.ID == todoItemId)
                .SingleOrDefault();
        }

        private int CreateItem(string text, User user)
        {
            Todo item = new Todo()
            {
                Text = text,
                User = user,
            };

            var t = _db.TodoList.Add(item);

            _db.SaveChanges();

            return t.Entity.ID;
        }

        private void ChangeItemStatus(Todo item)
        {
            if (item.Status == 0)
            {
                item.Status = 1;
            }
            else
            {
                item.Status = 0;
            }
            item.UpdatedAt = DateTime.Now;

            _db.SaveChanges();
        }

        private void DeleteItem(Todo item)
        {
            _db.TodoList.Remove(item);

            _db.SaveChanges();
        }
    }
}

