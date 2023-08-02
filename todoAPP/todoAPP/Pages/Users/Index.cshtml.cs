using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todoAPP.Models;

namespace todoAPP.Pages.Users
{
	public class IndexModel : PageModel
    {
        private readonly DataContext _db;

        public IEnumerable<User> Users { get; set; }

        public IndexModel(DataContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Users = _db.Users;
        }
    }
}
