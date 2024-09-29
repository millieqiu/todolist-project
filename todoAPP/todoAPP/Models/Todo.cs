using System;
using System.Collections.Generic;

namespace todoAPP.Models;

/// <summary>
/// 待辦事項
/// </summary>
public partial class Todo
{
    public int Idx { get; set; }

    public Guid Uid { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 備註
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTimeOffset CreateAt { get; set; }

    /// <summary>
    /// 最後更新時間
    /// </summary>
    public DateTimeOffset UpdateAt { get; set; }

    /// <summary>
    /// 預計執行時間
    /// </summary>
    public DateTimeOffset ExecuteAt { get; set; }

    /// <summary>
    /// 待辦事項列表順序
    /// </summary>
    public decimal GeneralTodoPosition { get; set; }

    /// <summary>
    /// 看板模式列表順序
    /// </summary>
    public decimal SwimlaneTodoPosition { get; set; }

    /// <summary>
    /// 使用者Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 標籤Id
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// SwimlaneId
    /// </summary>
    public Guid KanbanSwimlaneId { get; set; }

    public virtual KanbanSwimlane KanbanSwimlane { get; set; } = null!;

    public virtual UserTag Tag { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
