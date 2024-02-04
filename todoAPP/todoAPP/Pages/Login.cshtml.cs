using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoginModel : PageModel
    {
        private readonly UserService _user;

        public LoginModel(UserService user)
        {
            _user = user;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return Redirect("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost([FromBody] LoginRequestModel loginForm)
        {
            var user = await _user.QueryUser(loginForm.Username);

            if (PasswordHelper.ValidatePassword(user.Password, user.Salt, loginForm.Password))
            {
                await _user.SignInUser(HttpContext, user);
                return Redirect("/Index");
            }

            return Unauthorized();
        }
    }
}
