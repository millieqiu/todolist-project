using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
	public class PatchUsernameRequestModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Username { get; set; }
    }
}

