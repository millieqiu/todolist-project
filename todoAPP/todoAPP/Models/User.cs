using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class User
    {
        public User()
        {
            Todo = new HashSet<Todo>();
        }

        public int Idx { get; set; }
        public Guid Uid { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public int Role { get; set; }
        public string Avatar { get; set; } = null!;

        public virtual ICollection<Todo> Todo { get; set; }
    }
}
