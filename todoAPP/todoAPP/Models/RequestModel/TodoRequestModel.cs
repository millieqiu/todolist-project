using System.ComponentModel.DataAnnotations;

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

	public Guid TodoId { get; set; }
}

public class PatchTodoInfoRequestModel
{
	[StringLength(50, MinimumLength = 1)]
	public string Title { get; set; } = null!;

	[MaxLength(250)]
	public string Description { get; set; } = string.Empty;

	public DateTimeOffset? ExecuteAt { get; set; } = DateTimeOffset.UnixEpoch;
}

public class PatchTodoTagRequestModel
{
	public Guid TagId { get; set; } = Guid.Empty;
}

public class PatchGeneralTodoOrderRequestModel
{
	public Guid DragTodoId { get; set; }
	public Guid? DropPrevTodoId { get; set; }
	public Guid? DropNextTodoId { get; set; }
}

public class PatchSwimlaneTodoOrderRequestModel
{
	public Guid TodoId { get; set; }
	public Guid? KanbanSwimlaneId { get; set; }
	public Guid? DropPrevTodoId { get; set; }
	public Guid? DropNextTodoId { get; set; }
}