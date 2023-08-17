using System;
using System.ComponentModel.DataAnnotations;
using todoAPP.Models;

namespace todoAPP.RequestModel
{
	public class PatchAvatarRequestModel
    {
        [Required]
		public int UserID { get; set; }

        [Required]
        public IFormFile Avatar { get; set; }

    }
}

