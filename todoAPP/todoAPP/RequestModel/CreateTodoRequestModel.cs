using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
    public class CreateTodoRequestModel
    {
        [MaxLength(30)]
        public string Text { get; set; }
    }
}

