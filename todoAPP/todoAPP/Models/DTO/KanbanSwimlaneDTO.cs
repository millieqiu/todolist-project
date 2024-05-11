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

public class PatchKanbanSwimlaneOrderDTO
{
  public Guid DragSwimlaneId { get; set; }
  public Guid? DropPrevSwimlaneId { get; set; }
  public Guid? DropNextSwimlaneId { get; set; }
}

public class DeleteKanbanSwimlaneDTO
{
  public Guid KanbanSwimlaneId { get; set; }
}