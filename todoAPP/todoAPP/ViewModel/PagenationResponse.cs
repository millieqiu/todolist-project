using System;
using todoAPP.Models;

namespace todoAPP.ViewModel
{
    public class PagenationResponse
    {
        public double NumOfPages { get; set; }
        public int CurrentPage { get; set; }
        public List<TodoViewModel> List { get; set; }
    }
}

