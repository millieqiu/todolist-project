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
	public Guid UserID { get; set; }

	[EnumDataType(typeof(ERole))]
	public ERole RoleID { get; set; }

}