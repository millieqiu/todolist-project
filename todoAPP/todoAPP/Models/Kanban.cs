using System;
using System.Collections.Generic;

namespace todoAPP.Models;

/// <summary>
/// 看板
/// </summary>
public partial class Kanban
{
    public int Idx { get; set; }

    public Guid Uid { get; set; }

    /// <summary>
    /// 看板名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 使用者Id
    /// </summary>
    public Guid UserId { get; set; }

    public virtual ICollection<KanbanSwimlane> KanbanSwimlane { get; set; } = new List<KanbanSwimlane>();

    public virtual User User { get; set; } = null!;
}
