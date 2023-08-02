using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoAPP.ViewModel
{
	public class TodoViewModel
    {
        public int ID { get; set; }

        public byte Status { get; set; }

        public string Text { get; set; }
    }
}

