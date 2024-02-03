using System;
using System.Collections.Generic;

namespace todoAPP.ViewModel
{
  public class TodoViewModel
  {
    public Guid Uid { get; set; }
    public byte Status { get; set; }
    public string Text { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string Weather { get; set; } = null!;
  }
}
