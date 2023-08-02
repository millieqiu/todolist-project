using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todoAPP.Models;

namespace todoAPP.Pages.TodoList
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly DataContext _db;

        public IEnumerable<Todo> TodoList { get; set; }

        public Todo TodoForm { get; set; }

        public IndexModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            TodoList = _db.TodoList;
        }

        public async Task<IActionResult> OnPost()
        {
            await _db.TodoList.AddAsync(TodoForm);
            await _db.SaveChangesAsync();
            //return StatusCode(200);
            return RedirectToPage("Index");
        }
    }
}
