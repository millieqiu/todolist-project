using System;
using todoAPP.Migrations;
using todoAPP.Models;

namespace todoAPP.Services
{
    public class UserService
    {
        private readonly DataContext _db;
        private readonly AuthService _auth;
        private readonly RoleService _role;

        public UserService(DataContext db, AuthService auth, RoleService role)
        {
            _db = db;
            _auth = auth;
            _role = role;
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
                Role = _role.HasRole("user")
            };

            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}

