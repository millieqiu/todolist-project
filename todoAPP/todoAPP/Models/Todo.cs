using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoAPP.Models
{
	public class Todo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public string Text { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}

