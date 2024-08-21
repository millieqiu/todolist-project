using System.ComponentModel.DataAnnotations;

namespace todoAPP.Models.RequestModel;

public class CreateKanbanSwimlaneRequestModel
{
	[StringLength(30)]
	public string Name { get; set; } = string.Empty;
}

public class PatchKanbanSwimlaneNameRequestModel
{
	[StringLength(30)]
	public string Name { get; set; } = string.Empty;
}

public class PatchKanbanSwimlaneOrderRequestModel
{
	public Guid DragSwimlaneId { get; set; }
	public Guid? DropPrevSwimlaneId { get; set; }
	public Guid? DropNextSwimlaneId { get; set; }
}
