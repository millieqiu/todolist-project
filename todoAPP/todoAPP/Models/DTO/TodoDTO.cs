using todoAPP.Enums;

namespace todoAPP.DTO;

public class GetTodoListDTO
{
  public Guid UserId { get; set; }
}

public class CreateTodoDTO
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}

public class PatchTodoInfoDTO
{
  public Guid TodoId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTimeOffset ExecuteAt { get; set; } = DateTimeOffset.UnixEpoch;
}

public class PatchTodoSwimlaneDTO
{
  public Guid TodoId { get; set; }
  public Guid KanbanSwimlaneId { get; set; }
}

public class PatchUserTodoOrderDTO
{
  public EUpdateTodoOrderAction Action { get; set; }
  public Guid DragTodoId { get; set; }
  public Guid DropTodoId { get; set; }
}