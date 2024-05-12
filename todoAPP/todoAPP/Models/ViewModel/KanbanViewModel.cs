namespace todoAPP.ViewModel;

public class KanbanViewModel
{
	public Guid Uid { get; set; }
	public string Name { get; set; } = string.Empty;
	public IEnumerable<KanbanSwimlaneViewModel> KanbanSwimlaneList { get; set; } = new List<KanbanSwimlaneViewModel>();
}