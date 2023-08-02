using System;
using todoAPP.Models;

namespace todoAPP.ViewModel
{
    public class PagenationResponse
    {
        public int NumOfPages { get; set; }
        public List<TodoViewModel> List { get; set; }
    }
}

