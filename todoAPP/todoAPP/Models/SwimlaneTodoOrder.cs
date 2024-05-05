using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class SwimlaneTodoOrder
    {
        public int Idx { get; set; }
        public Guid TodoId { get; set; }
        public decimal Order { get; set; }

        public virtual Todo Todo { get; set; } = null!;
    }
}
