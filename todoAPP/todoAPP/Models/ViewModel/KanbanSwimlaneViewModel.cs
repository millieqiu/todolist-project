using todoAPP.Enums;
using TodoAPP_EnumService.Extensions;

namespace todoAPP.ViewModel;

public class KanbanSwimlaneListViewModel
{
	public Guid Uid { get; set; }
	public string Name { get; set; } = string.Empty;
}

public class KanbanSwimlaneViewModel
{
	public Guid Uid { get; set; }
	public byte Type { get; set; }
	public string TypeLabel
	{
		get
		{
			return ((EKanbanSwimlaneType)Type).GetDescriptionText();
		}
	}
	public string Name { get; set; } = string.Empty;
	public decimal Position { get; set; }
	public IEnumerable<TodoViewModel> TodoList { get; set; } = new List<TodoViewModel>();
}