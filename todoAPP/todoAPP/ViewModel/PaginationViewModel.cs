using System;

namespace todoAPP.ViewModel
{
    public class PaginationViewModel
    {
        public double NumOfPages { get; set; }

        public int CurrentPage { get; set; }

        public List<TodoViewModel> List { get; set; }
    }
}

