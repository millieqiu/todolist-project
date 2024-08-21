namespace todoAPP.Models.DTO;

public class CreateTodoDTO
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTimeOffset ExecuteAt { get; set; } = DateTimeOffset.UnixEpoch;
  public Guid TagId { get; set; }
  public Guid UserId { get; set; }
}

public class PatchTodoInfoDTO
{
  public Guid TodoId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTimeOffset ExecuteAt { get; set; } = DateTimeOffset.UnixEpoch;
}

public class PatchTodoTagDTO
{
  public Guid TodoId { get; set; }
  public Guid TagId { get; set; }
  public Guid UserId { get; set; }
}

public class PatchGeneralTodoOrderDTO
{
  public Guid UserId { get; set; }
  public Guid DragTodoId { get; set; }
  public Guid? DropPrevTodoId { get; set; }
  public Guid? DropNextTodoId { get; set; }
}

public class PatchSwimlaneTodoOrderDTO
{
  public Guid TodoId { get; set; }
  public Guid? KanbanSwimlaneId { get; set; }
  public Guid? DropPrevTodoId { get; set; }
  public Guid? DropNextTodoId { get; set; }
}

public class DeleteAlreadyDoneTodoDTO
{
  public Guid UserId { get; set; }
}