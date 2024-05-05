using System.ComponentModel.DataAnnotations;
using todoAPP.Enums;

namespace todoAPP.RequestModel;

public class GetTodoListRequestModel
{
  public Guid UserId { get; set; }
}

public class CreateTodoRequestModel
{
  [StringLength(50, MinimumLength = 1)]
  public string Title { get; set; } = null!;

  [MaxLength(250)]
  public string Description { get; set; } = string.Empty;

  public DateTimeOffset? ExecuteAt { get; set; } = null;
}

public class PatchTodoInfoRequestModel
{
  [StringLength(50, MinimumLength = 1)]
  public string Title { get; set; } = null!;

  [MaxLength(250)]
  public string Description { get; set; } = string.Empty;

  public DateTimeOffset? ExecuteAt { get; set; } = DateTimeOffset.UnixEpoch;
}

public class PatchTodoSwimlaneRequestModel
{
  public Guid KanbanSwimlaneId { get; set; }
}

public class PatchUserTodoOrderRequestModel
{
  [Range(1, 2)]
  public EUpdateTodoOrderAction Action { get; set; }
  public Guid DragTodoId { get; set; }
  public Guid DropTodoId { get; set; }
}

public class PatchSwimlaneTodoOrderRequestModel
{
  public Guid TodoId { get; set; }
  public Guid? KanbanSwimlaneId { get; set; }
  public Guid? DropPrevTodoId { get; set; }
  public Guid? DropNextTodoId { get; set; }
}