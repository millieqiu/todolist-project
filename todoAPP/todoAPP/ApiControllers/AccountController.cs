using Microsoft.AspNetCore.Mvc;
using todoAPP.RequestModel;
using todoAPP.Services;

namespace todoAPP.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly IUserService _user;

	public AccountController(IUserService user)
	{
		_user = user;
	}

	[Route("/Register")]
	public async Task<IActionResult> Register(RegisterRequestModel model)
	{
		await _user.CreateUser(model);
		return Ok();
	}
}
