using todoAPP.RequestModel;

namespace todoAPP.DTO;

public class GetTodoListDTO : PaginationRequestModel
{
  public Guid UserId { get; set; }
}

public class CreateTodoDTO
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}

public class PatchTodoSwimlaneDTO
{
  public Guid TodoId { get; set; }
  public Guid KanbanSwimlaneId { get; set; }
}