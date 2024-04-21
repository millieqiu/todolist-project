using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel;

public class GetTodoListRequestModel : PaginationRequestModel
{
    public Guid UserId { get; set; }
}

public class CreateTodoRequestModel
{
    [MaxLength(30)]
    public string Text { get; set; } = string.Empty;
    public Guid KanbanSwimlaneId { get; set; }
}

public class PatchTodoSwimlaneRequestModel
{
  public Guid KanbanSwimlaneId { get; set; }
}
