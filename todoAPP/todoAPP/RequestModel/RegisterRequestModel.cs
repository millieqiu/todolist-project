using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
    public class RegisterRequestModel
    {
        [StringLength(50, MinimumLength = 1)]
        [EmailAddress]
        public string Username { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 1)]
        public string Password { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 1)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 1)]
        public string Nickname { get; set; } = string.Empty;
    }
}