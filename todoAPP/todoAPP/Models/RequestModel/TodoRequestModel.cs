using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel;

public class GetTodoListRequestModel : PaginationRequestModel
{
  public Guid UserId { get; set; }
}

public class CreateTodoRequestModel
{
  [StringLength(50, MinimumLength = 1)]
  public string Title { get; set; } = null!;

  [MaxLength(250)]
  public string Description { get; set; } = string.Empty;
}

public class PatchTodoSwimlaneRequestModel
{
  public Guid KanbanSwimlaneId { get; set; }
}
