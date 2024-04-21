namespace todoAPP.DTO;

public class InitKanbanDTO
{
  public string Name { get; set; } = string.Empty;
  public string SwimlaneName { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}

public class GetKanbanListDTO
{
  public Guid UserId { get; set; }
}

public class GetDefaultKanbanDTO
{
  public Guid UserId { get; set; }
}