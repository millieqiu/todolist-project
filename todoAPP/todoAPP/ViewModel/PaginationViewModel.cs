using System;
using todoAPP.Models;

namespace todoAPP.ViewModel
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }

    public class PaginationViewModel
    {
        public double NumOfPages { get; set; }

        public int CurrentPage { get; set; }

        public List<Todo> List { get; set; }
    }
}

