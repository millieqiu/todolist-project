using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPP.Models
{
	public class User
    {
        [Key]
		public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Nickname { get; set; }
	}
}

