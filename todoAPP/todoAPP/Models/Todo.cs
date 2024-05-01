using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class Todo
    {
        public int Idx { get; set; }
        public Guid Uid { get; set; }
        public byte Status { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
        public DateTimeOffset ExecuteAt { get; set; }
        public Guid UserId { get; set; }
        public Guid KanbanSwimlaneId { get; set; }

        public virtual KanbanSwimlane KanbanSwimlane { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual UserTodoOrder? UserTodoOrder { get; set; }
    }
}
