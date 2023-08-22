using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using todoAPP.Migrations;
using todoAPP.Models;

namespace todoAPP.Services
{
    public class UserService
    {
        private readonly DataContext _db;
        private readonly AuthService _auth;
        private readonly FileService _file;

        public UserService(DataContext db, AuthService auth, FileService file)
        {
            _db = db;
            _auth = auth;
            _file = file;
        }

        public User? HasUser(int id)
        {
            return _db.Users
                .Where(x => x.ID == id)
                .SingleOrDefault();
        }

        public User? HasUser(string username)
        {
            return _db.Users
                .Where(x => x.Username == username)
                .SingleOrDefault();
        }

        public void CreateUser(string username, string password, string nickname)
        {
            byte[] salt = _auth.CreateSalt();

            User user = new User
            {
                Username = username,
                Password = _auth.PasswordGenerator(password, salt),
                Nickname = nickname,
                Salt = Convert.ToBase64String(salt),
                Role = Models.ERole.USER,
                Avatar = "default.jpeg"
            };

            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public Stream? GetAvatar(string fileName)
        {
            return _file.ReadFile(fileName);
        }

        async public Task EditAvatar(User user, IFormFile avatar)
        {
            if(string.IsNullOrEmpty(user.Avatar) == false && user.Avatar != "default.jpeg")
            {
                _file.RemoveFile(user.Avatar);
            }
            string fileName = await _file.WriteFile(avatar);
            user.Avatar = fileName;
            _db.SaveChanges();
        }

        async public Task SignInUser(HttpContext context, User user)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid,user.ID.ToString()), //使用者ID
                new Claim(ClaimTypes.NameIdentifier,user.Username),  //使用者帳號
                new Claim(ClaimTypes.Name,user.Nickname),  //使用者名稱
                new Claim("Avatar",user.Avatar)  //使用者圖像
            };

            if (user.Role == ERole.ADMIN)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = userPrincipal;
            var props = new AuthenticationProperties();
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, props);
        }
    }
}

