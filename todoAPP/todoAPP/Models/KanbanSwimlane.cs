using System;
using System.Collections.Generic;

namespace todoAPP.Models;

/// <summary>
/// Swimlane
/// </summary>
public partial class KanbanSwimlane
{
    public int Idx { get; set; }

    public Guid Uid { get; set; }

    /// <summary>
    /// 種類，0：預設，1：一般
    /// </summary>
    public byte Type { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 排序順序
    /// </summary>
    public decimal Position { get; set; }

    /// <summary>
    /// 看板Id
    /// </summary>
    public Guid KanbanId { get; set; }

    public virtual Kanban Kanban { get; set; } = null!;

    public virtual ICollection<Todo> Todo { get; set; } = new List<Todo>();
}
