using System.ComponentModel.DataAnnotations;
using todoAPP.Enums;

namespace todoAPP.Models.RequestModel;

public class PatchNicknameRequestModel
{
    [StringLength(50, MinimumLength = 1)]
	public string Nickname { get; set; } = null!;
}

public class PatchRoleRequestModel
{
	[EnumDataType(typeof(ERole))]
	public ERole Role { get; set; }
}