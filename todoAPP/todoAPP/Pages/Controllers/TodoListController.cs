using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;

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

        [HttpPost]
        public IActionResult Create([FromForm] Todo todoForm)
        {
            if(_db.TodoList.Find(todoForm.UserId) == null)
            {
                return BadRequest();
            }


            _db.TodoList.AddAsync(todoForm);
            _db.SaveChangesAsync();
            return new JsonResult(todoForm);
        }


    }
}

