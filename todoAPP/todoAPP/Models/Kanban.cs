using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class Kanban
    {
        public Kanban()
        {
            KanbanSwimlane = new HashSet<KanbanSwimlane>();
        }

        public int Idx { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; } = null!;
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<KanbanSwimlane> KanbanSwimlane { get; set; }
    }
}
