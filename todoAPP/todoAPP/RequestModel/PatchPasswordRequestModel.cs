using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
	public class PatchPasswordRequestModel
	{
        [Required]
        [MaxLength(50)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(50)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}

