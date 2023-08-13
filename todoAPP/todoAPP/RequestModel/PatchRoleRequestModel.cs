using System;
using System.ComponentModel.DataAnnotations;
using todoAPP.Models;

namespace todoAPP.RequestModel
{
	public class PatchRoleRequestModel
    {
		public int UserID { get; set; }

        [EnumDataType(typeof(ERole))]
        public Models.ERole RoleID { get; set; }

    }
}

