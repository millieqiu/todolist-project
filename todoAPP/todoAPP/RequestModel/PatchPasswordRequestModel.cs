using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
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

    public class PatchPasswordModel : PatchPasswordRequestModel
    {
        public Guid UserId { get; set; }
    }

}