using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using todoAPP.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace todoAPP.Pages.API
{
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
        public IActionResult ListAll(int lastId)
        {
            int count = _db.TodoList.Count();
            List<Todo> list = _db.TodoList.OrderBy(x => x.ID).ToList();

            return new JsonResult(list);
        }

        [HttpGet]
        public IActionResult Pagination(int lastId)
        {
            int count = _db.TodoList.Count();
            List<Todo> list = _db.TodoList.OrderBy(x => x.ID)
                .Where(x => x.ID > lastId)
                .Take(10).ToList();
            PagenationResponse response = new PagenationResponse();
            response.LastId = lastId;
            response.NumOfPages = count / 10 + 1;
            response.List = list;

            return new JsonResult(response);
        }

        [HttpPost]
        public IActionResult Create([FromForm] Todo todoForm)
        {
            if (_db.TodoList.Find(todoForm.UserId) == null)
            {
                return BadRequest();
            }


            var t = _db.TodoList.Add(todoForm);
            _db.SaveChangesAsync();
            return new JsonResult(t.Entity);
        }

        [HttpPut]
        public IActionResult ChangeStatus([FromForm] int id)
        {
            Todo? entity = _db.TodoList.Find(id);
            if (entity == null)
            {
                return BadRequest();
            }

            if (entity.Status == 0)
            {
                entity.Status = 1;
            }
            else
            {
                entity.Status = 0;
            }

            _db.SaveChangesAsync();
            return new JsonResult(entity);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var entity = _db.TodoList.Find(id);
            if (entity == null)
            {
                return BadRequest();
            }
            else
            {
                _db.TodoList.Remove(entity);
                _db.SaveChangesAsync();

            }

            return new JsonResult(entity);
        }

    }
}

