using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todoAPP.Services;

namespace todoAPP.Pages;

// [Authorize]
public class IndexModel : PageModel
{
    private readonly UserService _user;
    public string UserId = "";
    public string Username = "";
    public string Nickname = "";
    public string Avatar = "";
    public IndexModel(UserService user)
    {
        _user = user;
    }

    public IActionResult OnGet()
    {
        // InitUserInfo();
        return Page();
    }

    private void InitUserInfo()
    {
        UserId = _user.GetClaim(ClaimTypes.Sid);
        Username = _user.GetClaim(ClaimTypes.NameIdentifier);
        Nickname = _user.GetClaim(ClaimTypes.Name);
        Avatar = _user.GetClaim("Avatar");
    }
}

