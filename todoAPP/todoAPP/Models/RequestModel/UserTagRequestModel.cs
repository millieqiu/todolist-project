using System.ComponentModel.DataAnnotations;
using todoAPP.ValidationAttributes;

namespace todoAPP.RequestModel;

public class PatchUserTagNameRequestModel
{
	[GuidRequired]
	public Guid Uid { get; set; }
	
	[StringLength(15, MinimumLength = 1)]
	public string Name { get; set; } = string.Empty;
}
