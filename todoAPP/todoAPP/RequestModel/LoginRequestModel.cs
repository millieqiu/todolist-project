using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
    public class LoginRequestModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Password { get; set; } = string.Empty;
    }
}