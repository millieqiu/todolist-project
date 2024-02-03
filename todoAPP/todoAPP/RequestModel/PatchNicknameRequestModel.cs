using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
    public class PatchNicknameModel
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; } = string.Empty;
    }

    public class PatchNicknameRequestModel
    {
        [MaxLength(50)]
        public string Nickname { get; set; } = String.Empty;
    }
}