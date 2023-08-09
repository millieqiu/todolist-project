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

namespace todoAPP.Pages.API
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
        public IActionResult ListAll()
        {
            List<TodoViewModel> list = _db.TodoList
                .Where(x => x.User.ID == GetUserId())
                .Select(x => new TodoViewModel
                {
                    ID = x.ID,
                    Status = x.Status,
                    Text = x.Text
                })
                .ToList();

            return Ok(list);
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
                List = GetPaginatedData(page),
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
                    ErrMsg = "User dosen't exists.",
                };
                return BadRequest(err);
            }

            Todo item = new Todo()
            {
                Text = request.Text,
                User = user,
            };

            var t = _db.TodoList.Add(item);

            _db.SaveChanges();

            ModifyTodoViewModel response = GetModifyResponse(t.Entity, request.Page);

            return Ok(response);
        }

        [HttpPut]
        public IActionResult ChangeStatus(ModifyTodoRequestModel request)
        {
            Todo? entity = _db.TodoList.Include(x => x.User)
                .Where(x => x.User.ID == GetUserId() && x.ID == request.ID)
                .SingleOrDefault();

            if (entity == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "EditTodoItem",
                    Status = 1,
                    ErrMsg = $"Item id={request.ID} not found",
                };
                return BadRequest(err);
            }

            if (entity.Status == 0)
            {
                entity.Status = 1;
            }
            else
            {
                entity.Status = 0;
            }

            _db.SaveChanges();

            ModifyTodoViewModel response = GetModifyResponse(entity, request.Page);

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete(ModifyTodoRequestModel request)
        {
            Todo? entity = _db.TodoList.Include(x => x.User)
                .Where(x => x.User.ID == GetUserId() && x.ID == request.ID)
                .SingleOrDefault();

            if (entity == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "DeleteTodoItem",
                    Status = 1,
                    ErrMsg = $"Item id={request.ID} not found",
                };
                return BadRequest(err);
            }

            _db.TodoList.Remove(entity);

            _db.SaveChanges();

            ModifyTodoViewModel response = GetModifyResponse(entity, request.Page);

            return Ok(response);
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

        private List<TodoViewModel> GetPaginatedData(int page)
        {
            List<TodoViewModel> list = _db.TodoList
                .Where(x => x.User.ID == GetUserId())
                .Skip((page - 1) * 10)
                .Take(10)
                .Select(x => new TodoViewModel
                {
                    ID = x.ID,
                    Status = x.Status,
                    Text = x.Text
                }).ToList();

            return list;
        }

        private double GetNumOfPages()
        {
            int count = _db.TodoList.Where(x => x.User.ID == GetUserId()).Count();
            return Math.Ceiling(count / 10.0);
        }

        private ModifyTodoViewModel GetModifyResponse(Todo item, int currentPage)
        {

            if (currentPage < 1)
            {
                currentPage = 1;
            }

            return new ModifyTodoViewModel()
            {
                Item = new TodoViewModel()
                {
                    ID = item.ID,
                    Status = item.Status,
                    Text = item.Text,
                },
                Pagination = new PaginationViewModel()
                {
                    NumOfPages = GetNumOfPages(),
                    CurrentPage = currentPage,
                    List = GetPaginatedData(currentPage),
                },
            };
        }
    }
}

