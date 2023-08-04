using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todoAPP.Pages
{
    public class RegisterModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/TodoPage");
            }
            else
            {
                return Page();
            }
        }
    }
}
