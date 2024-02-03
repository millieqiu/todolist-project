using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todoAPP.Pages
{
    [Authorize]
    public class SettingPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
