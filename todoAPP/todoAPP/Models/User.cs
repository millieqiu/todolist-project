using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPP.Models
{
	public class User
    {
        [Key]
		public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(30)]
        public string Salt { get; set; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; }

        public ICollection<Todo> TodoList { get; set; }
	}
}

