using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todoAPP.Pages
{
    [Authorize]
    public class TodoPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
