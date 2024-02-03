using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
    public class PatchUsernameModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class PatchUsernameRequestModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Username { get; set; }
    }
}