using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class KanbanSwimlane
    {
        public KanbanSwimlane()
        {
            Todo = new HashSet<Todo>();
        }

        public int Idx { get; set; }
        public Guid Uid { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; } = null!;
        public decimal Position { get; set; }
        public Guid KanbanId { get; set; }

        public virtual Kanban Kanban { get; set; } = null!;
        public virtual ICollection<Todo> Todo { get; set; }
    }
}
