using System.ComponentModel.DataAnnotations;

namespace todoAPP.Models;

public class PatchUserTagNameRequestModel
{
    [StringLength(15, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}
