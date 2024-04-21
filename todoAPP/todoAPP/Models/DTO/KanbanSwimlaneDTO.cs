namespace todoAPP.DTO;

public class GetKanbanSwimlaneListDTO
{
  public Guid KanbanId { get; set; }
}

public class CreateKanbanSwimlaneDTO
{
  public Guid KanbanId { get; set; }
  public string Name { get; set; } = string.Empty;
}

public class PatchKanbanSwimlaneNameDTO
{
  public Guid KanbanSwimlaneId { get; set; }
  public string Name { get; set; } = string.Empty;
}

public class DeleteKanbanSwimlaneDTO
{
  public Guid KanbanSwimlaneId { get; set; }
}