namespace todoAPP.ViewModel;

public class UserInfoViewModel
{
	public Guid Uid { get; set; }

	public string Username { get; set; } = null!;

	public string Nickname { get; set; } = null!;

	public int Role { get; set; }
}

