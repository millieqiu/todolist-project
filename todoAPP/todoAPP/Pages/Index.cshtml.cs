using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todoAPP.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

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

