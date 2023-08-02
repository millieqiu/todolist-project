using System;
using todoAPP.Models;

namespace todoAPP.ViewModel
{
    public class PagenationResponse
    {
        public int LastId { get; set; }
        public int NumOfPages { get; set; }
        public List<Todo> List { get; set; }
    }
}

