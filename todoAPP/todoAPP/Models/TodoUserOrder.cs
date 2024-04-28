using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class TodoUserOrder
    {
        public Guid TodoId { get; set; }
        public Guid PrevId { get; set; }
        public Guid NextId { get; set; }

        public virtual Todo Todo { get; set; } = null!;
    }
}
