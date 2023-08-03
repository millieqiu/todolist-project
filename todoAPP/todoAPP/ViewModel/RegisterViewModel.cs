using System;
namespace todoAPP.ViewModel
{
	public class RegisterReqViewModel
	{
        public string Username { get; set; }

        public string Password { get; set; }

        public string Nickname { get; set; }
    }

    public class RegisterRespViewModel
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }
    }
}

