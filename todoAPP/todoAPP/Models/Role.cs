using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace todoAPP.Models
{
    [Index(nameof(Name), IsUnique = true)]
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

