using System;
using System.Collections.Generic;

namespace todoAPP.Models;

/// <summary>
/// 標籤
/// </summary>
public partial class UserTag
{
    public int Idx { get; set; }

    public Guid Uid { get; set; }

    /// <summary>
    /// 標籤種類，0：預設，1：一般
    /// </summary>
    public byte Type { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 顏色
    /// </summary>
    public string Color { get; set; } = null!;

    /// <summary>
    /// 使用者Id
    /// </summary>
    public Guid UserId { get; set; }

    public virtual ICollection<Todo> Todo { get; set; } = new List<Todo>();

    public virtual User User { get; set; } = null!;
}
