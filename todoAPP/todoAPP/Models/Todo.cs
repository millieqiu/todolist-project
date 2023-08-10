using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace todoAPP.Models
{
	public class Todo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}

