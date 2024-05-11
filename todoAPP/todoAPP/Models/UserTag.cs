using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class UserTag
    {
        public UserTag()
        {
            Todo = new HashSet<Todo>();
        }

        public int Idx { get; set; }
        public Guid Uid { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; } = null!;
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Todo> Todo { get; set; }
    }
}
