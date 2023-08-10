using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace todoAPP.Models
{
	public class Role
	{
        [Key]
		public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
	}
}

