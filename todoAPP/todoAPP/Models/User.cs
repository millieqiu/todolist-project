using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace todoAPP.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
		public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(30)]
        public string Salt { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nickname { get; set; }

        public ICollection<Todo> TodoList { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }
    }
}

