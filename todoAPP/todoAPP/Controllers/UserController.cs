using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using todoAPP.ViewModel;

namespace todoAPP.Pages.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly DataContext _db;


        public UserController(DataContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterReqViewModel registerForm)
        {
            List<User> list = _db.Users
                .Where(x => x.Username == registerForm.Username)
                .ToList();
            if (list.Count > 0)
            {
                return BadRequest("Username exist");
            }

            byte[] salt = CreateSalt();
            string derivedPassword = PasswordGenerator(registerForm.Password, salt);

            User user = new User
            {
                Username = registerForm.Username,
                Password = derivedPassword,
                Nickname = registerForm.Nickname,
                Salt = Convert.ToBase64String(salt)
            };

            var result = _db.Users.Add(user);
            _db.SaveChanges();
            RegisterRespViewModel respObj = new RegisterRespViewModel
            {
                ID = result.Entity.ID,
                Username = result.Entity.Username,
                Nickname = result.Entity.Nickname
            };
            return Ok(respObj);
        }

        private static string PasswordGenerator(string password, byte[] salt)
        {
            byte[] key = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,//鹽
                prf: KeyDerivationPrf.HMACSHA512,//偽隨機函數
                iterationCount: 1000,//雜湊執行次數
                numBytesRequested: 256 / 8
                );
            return Convert.ToBase64String(key);
        }

        private static byte[] CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}

