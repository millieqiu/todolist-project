using System;
using System.Collections.Generic;

namespace todoAPP.Models;

/// <summary>
/// 使用者
/// </summary>
public partial class User
{
    public int Idx { get; set; }

    public Guid Uid { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 暱稱
    /// </summary>
    public string Nickname { get; set; } = null!;

    /// <summary>
    /// 鹽
    /// </summary>
    public string Salt { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    public int Role { get; set; }

    /// <summary>
    /// 頭像
    /// </summary>
    public string Avatar { get; set; } = null!;

    public virtual ICollection<Kanban> Kanban { get; set; } = new List<Kanban>();

    public virtual ICollection<Todo> Todo { get; set; } = new List<Todo>();

    public virtual ICollection<UserTag> UserTag { get; set; } = new List<UserTag>();
}
