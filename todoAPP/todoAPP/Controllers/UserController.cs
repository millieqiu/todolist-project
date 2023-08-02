using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace todoAPP.Pages.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly DataContext _db;


        public UserController(DataContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Register([FromForm] User userForm)
        {
            if (_db.Users.Where(x=>x.Username == userForm.Username) != null)
            {
                return BadRequest("Username exist");
            }


            var result = _db.Users.Add(userForm);
            _db.SaveChangesAsync();
            return new JsonResult(result.Entity);
        }
    }
}

