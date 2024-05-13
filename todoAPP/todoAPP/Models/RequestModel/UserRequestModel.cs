using System.ComponentModel.DataAnnotations;
using todoAPP.Enums;

namespace todoAPP.RequestModel;

public class PatchNicknameRequestModel
{
	[MaxLength(50)]
	public string Nickname { get; set; } = String.Empty;
}

public class PatchRoleRequestModel
{
	[EnumDataType(typeof(ERole))]
	public ERole Role { get; set; }

}