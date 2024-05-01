using System.ComponentModel;

namespace todoAPP.Enums;

public enum ETodoStatus
{
  [Description("未完成")]
  UNDONE = 0,

  [Description("已完成")]
  DONE = 1,
}
