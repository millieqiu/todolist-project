using System;
using todoAPP.Migrations;
using todoAPP.Models;

namespace todoAPP.Services
{
    public class UserService
    {
        private readonly DataContext _db;
        private readonly AuthService _auth;

        public UserService(DataContext db, AuthService auth)
        {
            _db = db;
            _auth = auth;
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
                Role = Models.ERole.USER
            };

            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}

