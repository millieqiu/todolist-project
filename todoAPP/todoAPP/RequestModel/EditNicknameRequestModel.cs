using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
	public class EditNicknameRequestModel
    {
        [MaxLength(50)]
        public string Nickname { get; set; }
	}
}

