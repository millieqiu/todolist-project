using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using todoAPP.ViewModel;

namespace todoAPP.RequestModel
{
	public class RegisterRequestModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nickname { get; set; }
    }
}

