using System;
using System.Collections.Generic;
using System.IO;
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
    }
}

