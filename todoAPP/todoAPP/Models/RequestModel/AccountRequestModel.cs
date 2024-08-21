using System.ComponentModel.DataAnnotations;

namespace todoAPP.Models.RequestModel;

public class LoginRequestModel
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = null!;
}

public class RegisterRequestModel
{
    [StringLength(50, MinimumLength = 1)]
    [EmailAddress]
    public string Username { get; set; } = null!;

    [StringLength(50, MinimumLength = 1)]
    public string Password { get; set; } = null!;

    [StringLength(50, MinimumLength = 1)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;

    [StringLength(50, MinimumLength = 1)]
    public string Nickname { get; set; } = null!;
}

public class PatchUsernameRequestModel
{
    [StringLength(50, MinimumLength = 1)]
    [EmailAddress]
    public string Username { get; set; } = null!;
}

public class PatchPasswordRequestModel
{
    [StringLength(50, MinimumLength = 1)]
    public string OldPassword { get; set; } = null!;

    [StringLength(50, MinimumLength = 1)]
    public string NewPassword { get; set; } = null!;

    [StringLength(50, MinimumLength = 1)]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = null!;
}