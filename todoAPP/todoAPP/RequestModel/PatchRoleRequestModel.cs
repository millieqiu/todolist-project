using System.ComponentModel.DataAnnotations;
using todoAPP.Enums;

namespace todoAPP.RequestModel
{
    public class PatchRoleRequestModel
    {
        public Guid UserID { get; set; }

        [EnumDataType(typeof(ERole))]
        public ERole RoleID { get; set; }

    }
}