using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel;

public class LoginRequestModel
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequestModel
{
    [StringLength(50, MinimumLength = 1)]
    [EmailAddress]
    public string Username { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 1)]
    public string Password { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 1)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 1)]
    public string Nickname { get; set; } = string.Empty;
}

public class PatchUsernameRequestModel
{
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Username { get; set; }
}

public class PatchPasswordRequestModel
{
    [Required]
    [MaxLength(50)]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}