using todoAPP.Enums;
using TodoAPP_EnumService.Extensions;

namespace todoAPP.Models.ViewModel;

public class UserTagViewModel
{
    public Guid Uid { get; set; }

    public byte Type { get; set; }

    public string TypeLabel
    {
        get
        {
            return ((EUserTagType)Type).GetDescriptionText();
        }
    }

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
