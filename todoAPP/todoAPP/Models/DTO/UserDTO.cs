using todoAPP.Enums;

namespace todoAPP.Models.DTO;

public class PatchUsernameDTO
{
	public Guid UserId { get; set; }
	public string Username { get; set; } = string.Empty;
}

public class PatchNicknameDTO
{
	public Guid UserId { get; set; }
	public string Nickname { get; set; } = string.Empty;
}

public class PatchPasswordDTO
{
	public Guid UserId { get; set; }
	public string OldPassword { get; set; } = string.Empty;
	public string NewPassword { get; set; } = string.Empty;
}

public class PatchAvatarDTO
{
	public Guid UserId { get; set; }
	public IFormFile File { get; set; } = null!;
}

public class PatchUserRoleDTO
{
	public Guid UserId { get; set; }
	public ERole Role { get; set; }
}

public class DeleteUserDTO
{
	public Guid UserId { get; set; }
}