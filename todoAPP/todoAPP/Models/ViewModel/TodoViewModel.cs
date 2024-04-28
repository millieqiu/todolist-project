namespace todoAPP.ViewModel;

public class TodoViewModel
{
  public Guid Uid { get; set; }
  public byte Status { get; set; }
  public string Title { get; set; } = null!;
  public string Description { get; set; } = null!;
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
  public string Weather { get; set; } = null!;
  public Guid PrevId { get; set; }
  public Guid NextId { get; set; }
}
