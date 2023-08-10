using System;
using todoAPP.Models;

namespace todoAPP.ViewModel
{
    public class PaginationViewModel
    {
        public double NumOfPages { get; set; }

        public int CurrentPage { get; set; }

        public List<Todo> List { get; set; }
    }
}

