using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly UserService _user;

        public RegisterModel(UserService user)
        {
            _user = user;
        }

        public IActionResult OnGet()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(RegisterRequestModel model)
        {
            await _user.CreateUser(model);
            return Redirect("/Login");
        }
    }
}
