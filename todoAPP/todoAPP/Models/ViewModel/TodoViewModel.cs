using todoAPP.Enums;
using todoAPP.Models;
using TodoAPP_EnumService.Extensions;

namespace todoAPP.ViewModel;

public class TodoViewModel
{
  public Guid Uid { get; set; }
  public byte Status { get; set; }
  public string StatusLabel
  {
    get
    {
      return ((ETodoStatus)Status).GetDescriptionText();
    }
  }
  public string Title { get; set; } = null!;
  public string Description { get; set; } = null!;
  public DateTimeOffset CreateAt { get; set; }
  public DateTimeOffset UpdateAt { get; set; }
  private DateTimeOffset? executeAt { get; set; }
  public DateTimeOffset? ExecuteAt
  {
    get { return executeAt == DateTimeOffset.UnixEpoch ? null : executeAt; }
    set { executeAt = value; }
  }
  public decimal Position { get; set; }
  public UserTagViewModel? Tag { get; set; }
}
